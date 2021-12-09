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
        private Dictionary<Core.Character.Class.ClassType, GameObject> _groupObjects;
        public static Core.Character.Class.ClassType[] AvailableClasses { get; private set; }

        void Start()
        {
            AvailableClasses = Core.Global.GlobalData.Inventory
                .GetKeyItems<Core.Items.ClassMemoryData>("Class")
                .Select(item => item.Class).ToArray();
            _groupObjects = Core.Character.Patapons.PataponGroupGenerator.Generate(transform, AvailableClasses);

            foreach (var obj in _groupObjects.Values)
            {
                foreach (var pon in obj.GetComponentsInChildren<PataponData>())
                {
                    pon.gameObject.AddComponent<Common.Navigator.SpriteSelectable>();
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
            Common.SceneLoadingAction.Create("Battle", true);
            Exit(_onEnter);
        }
        public void GoBack()
        {
            Common.SceneLoadingAction.Create("Patapolis", false);
            Exit(_onCancel);
        }
        private void Exit(AudioClip sound)
        {
            //And save data to static!
            Core.Global.GlobalData.Sound.Play(sound);
        }

    }
}
