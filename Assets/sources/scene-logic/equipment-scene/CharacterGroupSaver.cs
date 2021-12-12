using PataRoad.Core.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterGroupSaver : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _onEnter;
        [SerializeField]
        private AudioClip _onCancel;
        [SerializeField]
        private CharacterGroupNavigator _groupNav;
        private Dictionary<Core.Character.Class.ClassType, GameObject> _groupObjects;
        private Dictionary<GameObject, PataponData[]> _pataponDataMap;
        public static Core.Character.Class.ClassType[] AvailableClasses { get; private set; }

        void Start()
        {
            AvailableClasses = Core.Global.GlobalData.Inventory
                .GetKeyItems<Core.Items.ClassMemoryData>("Class")
                .Select(item => item.Class).ToArray();
            _groupObjects = Core.Character.Patapons.PataponGroupGenerator.Generate(transform, AvailableClasses);
            _pataponDataMap = _groupObjects.ToDictionary(kv => kv.Value, kv => kv.Value.GetComponentsInChildren<PataponData>().OrderBy(data => data.IndexInGroup).ToArray());

            foreach (var obj in _groupObjects.Values)
            {
                bool init = false;
                foreach (var pon in obj.GetComponentsInChildren<PataponData>())
                {
                    pon.gameObject.AddComponent<Common.Navigator.SpriteSelectable>();
                    if (!init)
                    {
                        obj.AddComponent<CharacterGroupData>().Init(pon.Type);
                        init = true;
                    }
                }
            }
            GetComponent<CharacterGroupNavigator>().Init();
        }
        public GameObject GetGroup(Core.Character.Class.ClassType type) => _groupObjects[type];
        public GameObject LoadGroup(Core.Character.Class.ClassType type)
        {
            var obj = _groupObjects[type];
            obj.SetActive(true);
            Animate(obj);
            return obj;
        }
        public PataponData GetPataponDataInIndex(Core.Character.Class.ClassType type, int index) =>
            _pataponDataMap[_groupObjects[type]][index];
        public GameObject HideGroup(Core.Character.Class.ClassType type)
        {
            var obj = _groupObjects[type];
            obj.SetActive(false);
            return obj;
        }
        public void Animate(GameObject groupObject)
        {
            foreach (var patapon in groupObject.GetComponentsInChildren<PataponData>())
            {
                patapon.Animator.Play("walk");
                if (patapon.Type == Core.Character.Class.ClassType.Toripon)
                {
                    patapon.Animator.Play("tori-fly-stop");
                }
            }
        }
        public void StartMission()
        {
            var window = Common.GameDisplay.ConfirmDialog.Create("Start the mission?", _groupNav, () =>
            {
                StopAllCoroutines();
                Common.SceneLoadingAction.Create("Battle", true);
                Exit(_onEnter);
            });
            window.IsScreenChange = true;
            //this will take time and less important so works separately
            StartCoroutine(CheckStatus());
            System.Collections.IEnumerator CheckStatus()
            {
                bool isWarned = false;
                string ignoreMessage = "------------------------------ \nIgnore these warning message above if it's intended.";
                yield return null;
                var rect = window.GetComponent<RectTransform>();
                var dialogContent = window.Content;

                if (Core.Global.GlobalData.PataponInfo.ClassCount < Core.Character.Patapons.Data.PataponInfo.MaxPataponGroup
                    && AvailableClasses.Length > Core.Global.GlobalData.PataponInfo.ClassCount)
                {
                    AddMessage("\n[!] Squad is not full");
                }
                yield return null;

                foreach (var classType in Core.Global.GlobalData.PataponInfo.CurrentClasses)
                {
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop - real time update
                    (var weaponName, var protectorName) = Core.Character.Class.ClassMetaData.GetWeaponAndProtectorName(classType);
                    if (!isEquipped(classType, weaponName))
                    {
                        AddMessage($"\n[!] Can optimize {weaponName} for {classType}");
                    }
                    if (!isEquipped(classType, protectorName))
                    {
                        AddMessage($"\n[!] Can optimize {protectorName} for {classType}");
                    }
                    yield return null;
                }
                yield return null;
                void AddMessage(string message)
                {
                    dialogContent.text += message;
                    if (!isWarned)
                    {
                        var obj = new GameObject();
                        obj.transform.parent = dialogContent.transform;
                        var txt = obj.AddComponent<UnityEngine.UI.Text>();
                        txt.text = ignoreMessage;
                        isWarned = true;
                    }
                    UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                }
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }
            bool isEquipped(Core.Character.Class.ClassType classType, string equipmentName)
            {
                if (equipmentName == null) return true;
                var bestEquipmentIndex = Core.Global.GlobalData.Inventory.GetBestEquipmentIndex(equipmentName);
                if (bestEquipmentIndex < 1) return true;
                var bestEquipment = Core.Items.ItemLoader.GetItem<Core.Items.EquipmentData>(Core.Items.ItemType.Equipment, equipmentName, bestEquipmentIndex);
                return Core.Global.GlobalData.PataponInfo.IsEquippedInside(classType, bestEquipment);
            }
        }
        public void GoBack()
        {
            var window = Common.GameDisplay.ConfirmDialog.Create("Go back to the Patapolis?", _groupNav, () =>
            {
                Common.SceneLoadingAction.Create("Patapolis", false);
                Exit(_onCancel);
            });
            window.IsScreenChange = true;
        }
        private void Exit(AudioClip sound)
        {
            Core.Global.GlobalData.Sound.PlayGlobal(sound);
        }

    }
}
