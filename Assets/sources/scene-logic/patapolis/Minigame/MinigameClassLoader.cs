using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameClassLoader : MonoBehaviour
    {
        [Header("Class GameObjects")]
        [SerializeField]
        private GameObject _yaripon;
        [SerializeField]
        private GameObject _tatepon;
        [SerializeField]
        private GameObject _yumipon;
        [SerializeField]
        private GameObject _kibapon;
        [SerializeField]
        private GameObject _dekapon;
        [SerializeField]
        private GameObject _megapon;
        [SerializeField]
        private GameObject _toripon;
        [SerializeField]
        private GameObject _robopon;
        [SerializeField]
        private GameObject _mahopon;
        private Core.Character.Class.ClassType[] _availableClasses;

        private void Start()
        {
            _availableClasses = Core.Global.GlobalData.CurrentSlot.Inventory
                .GetKeyItems<Core.Items.ClassMemoryData>("Class")
                .Select(item => item.Class).ToArray();

            OpenIfExists(Core.Character.Class.ClassType.Yaripon, _yaripon);
            OpenIfExists(Core.Character.Class.ClassType.Tatepon, _tatepon);
            OpenIfExists(Core.Character.Class.ClassType.Yumipon, _yumipon);
            OpenIfExists(Core.Character.Class.ClassType.Kibapon, _kibapon);
            OpenIfExists(Core.Character.Class.ClassType.Dekapon, _dekapon);
            OpenIfExists(Core.Character.Class.ClassType.Megapon, _megapon);
            OpenIfExists(Core.Character.Class.ClassType.Toripon, _toripon);
            OpenIfExists(Core.Character.Class.ClassType.Robopon, _robopon);
            OpenIfExists(Core.Character.Class.ClassType.Mahopon, _mahopon);
        }
        private void OpenIfExists(Core.Character.Class.ClassType classType, GameObject classObject)
        {
            classObject.SetActive(System.Array.IndexOf(_availableClasses, classType) >= 0);
        }
    }
}
