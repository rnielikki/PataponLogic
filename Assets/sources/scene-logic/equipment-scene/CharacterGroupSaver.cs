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
            AvailableClasses = Core.Global.GlobalData.CurrentSlot.Inventory
                .GetKeyItems<Core.Items.ClassMemoryData>("Class")
                .Select(item => item.Class).ToArray();
            _groupObjects = Core.Character.Patapons.PataponGroupGenerator.Generate(transform, AvailableClasses);
            _pataponDataMap = _groupObjects.ToDictionary(
                kv => kv.Value,
                kv => kv.Value.GetComponentsInChildren<PataponData>().OrderBy(data => data.IndexInGroup).ToArray());

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
            if (Core.Global.GlobalData.CurrentSlot.MapInfo.NextMap == null)
            {
                Common.GameDisplay.ConfirmDialog.Create("Error: Map data doesn't exist so we cannot deploy!")
                    .HideOkButton()
                    .SetCancelAction(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Patapolis"))
                    .SelectCancel();
                Core.Global.GlobalData.Sound.PlayBeep();
                return;
            }
            var window = Common.GameDisplay.ConfirmDialog.Create("Start the mission?")
                .SetTargetToResume(_groupNav)
                .SetOkAction(() =>
                {
                    StopAllCoroutines();
                    Common.GameDisplay.SceneLoadingAction.ChangeScene("Battle", true);
                    Exit(_onEnter);
                })
                .SelectOk();
            window.IsScreenChange = true;
            //this will take time and less important so works separately
            StartCoroutine(CheckStatus());
            System.Collections.IEnumerator CheckStatus()
            {
                yield return null;

                if (Core.Global.GlobalData.CurrentSlot.PataponInfo.ClassCount < Core.Character.Patapons.Data.PataponInfo.MaxPataponGroup
                    && AvailableClasses.Length > Core.Global.GlobalData.CurrentSlot.PataponInfo.ClassCount)
                {
                    window.AppendText("[!] Squad is not full");
                }
                yield return null;

                foreach (var classType in Core.Global.GlobalData.CurrentSlot.PataponInfo.CurrentClasses)
                {
                    (var weaponName, var protectorName) = Core.Character.Class.ClassAttackEquipmentData
                        .GetWeaponAndProtectorName(classType);
                    if (!isEquipped(classType, Core.Character.Equipments.EquipmentType.Weapon, weaponName))
                    {
                        window.AppendText($"[!] Can optimize {weaponName} for {classType}");
                    }
                    if (!isEquipped(classType, Core.Character.Equipments.EquipmentType.Protector, protectorName))
                    {
                        window.AppendText($"[!] Can optimize {protectorName} for {classType}");
                    }
                    yield return null;
                }
                yield return null;
            }
            bool isEquipped(
                Core.Character.Class.ClassType classType,
                Core.Character.Equipments.EquipmentType equipmentType,
                string equipmentName)
            {
                if (equipmentName == null) return true;
                var bestEquipmentIndex = Core.Global.GlobalData.CurrentSlot.Inventory.GetBestEquipmentIndex(equipmentName);
                if (bestEquipmentIndex < 1) return true;
                var bestEquipment = Core.Items.ItemLoader
                    .GetItem<Core.Items.EquipmentData>(Core.Items.ItemType.Equipment, equipmentName, bestEquipmentIndex);
                return Core.Global.GlobalData.CurrentSlot.PataponInfo
                    .HasBestEquipmentInside(classType, equipmentType, bestEquipment.LevelGroup);
            }
        }
        public void GoBack()
        {
            var window = Common.GameDisplay.ConfirmDialog.Create("Go back to the Patapolis?")
                .SetTargetToResume(_groupNav)
                    .SetOkAction(() =>
                    {
                        Common.GameDisplay.SceneLoadingAction.ChangeScene("Patapolis");
                        Exit(_onCancel);
                    })
                    .SelectOk();
            window.IsScreenChange = true;
        }
        private void Exit(AudioClip sound)
        {
            Core.Global.GlobalData.Sound.PlayGlobal(sound);
        }
    }
}
