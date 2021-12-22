using PataRoad.Common.Navigator;
using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public abstract class SummaryMenu<T> : MonoBehaviour where T : SummaryElement
    {
        [SerializeField]
        protected AudioClip _selectSound;
        protected ActionEventMap _actionEvent;

        //It really selects nothing. just look like it's selected.
        protected int _index;
        protected T[] _activeNavs;
        public T Current { get; protected set; }

        protected void Init()
        {
            _actionEvent = GetComponent<ActionEventMap>();
            foreach (var elem in GetComponentsInChildren<T>())
            {
                elem.Init();
            }
        }

        private void OnDisable()
        {
            SetInactive();
            Current?.MarkAsDeselected();
        }
        protected virtual void WhenDisabled() { }
        public void SetInactive()
        {
            if (_activeNavs != null)
            {
                foreach (var elem in _activeNavs)
                {
                    elem.enabled = false;
                }
            }
            _actionEvent.enabled = false;
        }
        public virtual void ResumeToActive()
        {
            if (_activeNavs != null)
            {
                Current?.MarkAsDeselected();
                foreach (var elem in _activeNavs)
                {
                    elem.enabled = true;
                }
            }
            _actionEvent.enabled = true;
        }
        public virtual void MoveTo(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting) return;
            var directionY = context.ReadValue<Vector2>().y;
            MoveTo(directionY);
        }
        protected void MoveTo(float directionY)
        {
            var index = _index;
            if (directionY < -0.5 || directionY > 0.5)
            {
                int dir = directionY < 0 ? -1 : 1;
                index = (index + dir * -1 + _activeNavs.Length) % _activeNavs.Length;
                Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            }
            MarkIndex(index);
        }
        protected void MarkIndex(int index)
        {
            var oldCurrent = Current;
            oldCurrent?.MarkAsDeselected();
            _index = index;
            Current = _activeNavs[_index];
            Current.MarkAsSelected();
        }
    }
}
