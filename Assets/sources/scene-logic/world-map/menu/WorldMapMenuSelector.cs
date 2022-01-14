using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    class WorldMapMenuSelector : MonoBehaviour
    {
        [SerializeField]
        Text _leftGuide;
        [SerializeField]
        Text _rightGuide;
        private WorldMapMenu[] _menus;
        private int _menuLength;
        private int _index;
        private WorldMapMenu _current => _menus[_index];

        [SerializeField]
        WorldMapSelector _list;
        [SerializeField]
        AudioClip _selectSound;

        private void Start()
        {
            Core.Global.GlobalData.GlobalInputActions.TryGetAllActionBindingNames("UI/LR", out var guides);
            if (guides.ContainsKey("positive"))
            {
                _rightGuide.text = $"{guides["positive"]} -->";
            }
            if (guides.ContainsKey("negative"))
            {
                _leftGuide.text = $"<-- {guides["negative"]}";
            }
            _menus = GetComponentsInChildren<WorldMapMenu>();
            _menuLength = _menus.Length;
            _current.MarkAsCurrentAndGetFilter();
        }
        public void Move(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            float value = context.ReadValue<float>();
            if (value == 0) return;
            int offset = value > 0 ? 1 : -1;
            _current.MarkAsNonCurrent();
            _index = (_index + _menuLength + offset) % _menuLength;
            var type = _current.MarkAsCurrentAndGetFilter();
            switch (type)
            {
                case WorldMapFilterType.All:
                    _list.ShowAll();
                    break;
                case WorldMapFilterType.NotCleared:
                    _list.ShowNotCleared();
                    break;
                default:
                    var typeAsNumber = (int)type;
                    if (System.Enum.IsDefined(typeof(Core.Map.MapType), typeAsNumber))
                    {
                        _list.Filter((Core.Map.MapType)typeAsNumber);
                    }
                    break;
            }
        }
        public void GoToPatapolis()
        {
            Common.GameDisplay.SceneLoadingAction.Create("Patapolis").ChangeScene();
        }
    }
}
