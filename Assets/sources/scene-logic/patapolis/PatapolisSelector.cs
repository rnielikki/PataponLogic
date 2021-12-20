using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    public class PatapolisSelector : MonoBehaviour
    {
        [SerializeField]
        private Common.Navigator.ActionEventMap _actionEventMap;
        [SerializeField]
        CameraSmoothMover _cameraMover;
        private PatapolisSelection[] _selections;
        private int _selectionsLength;
        private int _index;
        private PatapolisSelection _current => _selections[_index];
        private void Start()
        {
            _selections = GetComponentsInChildren<PatapolisSelection>();
            foreach (var selection in _selections) selection.Init();
            _selectionsLength = _selections.Length;
            _selections[0].Select();
        }
        private void OnEnable()
        {
            _actionEventMap.enabled = true;
        }
        private void OnDisable()
        {
            _actionEventMap.enabled = false;
        }
        public void Perform() => _current.Perform();
        public void Move(Object caller, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var res = context.ReadValue<float>();
            if (res != 0) Move(res < 0);
        }
        private void Move(bool right)
        {
            if (_cameraMover.IsMoving) return;
            var oldCurrent = _current;
            if (right && _index < _selectionsLength - 1)
            {
                _index++;
            }
            else if (!right && _index > 0)
            {
                _index--;
            }
            else return;

            oldCurrent.Deselect();
            _cameraMover.MoveTo(_current.transform.position.x);
            _current.Select();
        }
    }
}
