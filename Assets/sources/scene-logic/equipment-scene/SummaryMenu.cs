using PataRoad.Common.Navigator;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public abstract class SummaryMenu<T> : MonoBehaviour where T : SummaryElement
    {
        [SerializeField]
        protected AudioSource _selectSoundSource;
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
            var directionY = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
            MoveTo(directionY);
        }
        protected void MoveTo(int directionY)
        {
            var index = _index;
            if (directionY == 1 || directionY == -1)
            {
                index = (index + directionY * -1 + _activeNavs.Length) % _activeNavs.Length;
            }
            MarkIndex(index);
            if (_selectSound != null) _selectSoundSource.PlayOneShot(_selectSound);
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
