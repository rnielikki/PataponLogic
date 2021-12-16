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

        private void Start()
        {
            Core.Global.GlobalData.TryGetAllActionBindingNames("UI/LR", out var guides);
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
            float value = context.ReadValue<float>();
            if (value == 0) return;
            int offset = value > 0 ? 1 : -1;
            _current.MarkAsNonCurrent();
            _index = (_index + _menuLength + offset) % _menuLength;
            var type = _current.MarkAsCurrentAndGetFilter();
            if (type != null) _list.Filter(type.Value);
            else _list.ShowAll();
        }
        public void GoToPatapolis()
        {
            Common.SceneLoadingAction.Create("Patapolis", false);
        }
    }
}
