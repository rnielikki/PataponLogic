using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.Common.Navigator
{
    public class SpriteNavigator : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _backgroundObject;
        [SerializeField]
        protected bool _useSprite;
        [SerializeField]
        protected Sprite _background;
        [SerializeField]
        protected AudioClip _selectSound;
        [SerializeField]
        protected AudioSource _audioSource;
        [SerializeField]
        protected UnityEngine.Events.UnityEvent<SpriteSelectable> _onSelected = new UnityEngine.Events.UnityEvent<SpriteSelectable>();
        [SerializeField]
        protected Vector2 _positionOffset;
        [SerializeField]
        protected bool _preserveIndexOnDeselected;
        public bool PreserveIndexOnDeselected { get; set; }

        protected ActionEventMap _map;

        public SpriteSelectable Current => _navs[_index];
        protected List<SpriteSelectable> _navs = new List<SpriteSelectable>();
        protected int _index;
        // Start is called before the first frame update
        public virtual void Init()
        {
            PreserveIndexOnDeselected = _preserveIndexOnDeselected;
            foreach (Transform child in transform)
            {
                var comp = child.gameObject.AddComponent<SpriteSelectable>();
                if (_useSprite) comp.Init(this, _background, _positionOffset, _onSelected);
                else comp.Init(this, _backgroundObject, _positionOffset, _onSelected);
                _navs.Add(comp);
            }
            Current.SelectThis();
        }
        /// <summary>
        /// Show as selected, even it's really deselected. This includes deselecting and disabling the navigator.
        /// </summary>
        public void Freeze()
        {
            Current.Freeze();
            enabled = false;
        }

        public virtual void MoveTo(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                    _index = (_index + 1) % _navs.Count;
                    break;
                case MoveDirection.Right:
                    _index = (_index - 1 + _navs.Count) % _navs.Count;
                    break;
                default:
                    return;
            }
            Current.SelectThis();
            if (_selectSound != null) _audioSource.PlayOneShot(_selectSound);
        }
        private void OnEnable()
        {
            foreach (var spriteSelectable in _navs)
            {
                spriteSelectable.enabled = true;
            }
            if (_map != null) _map.enabled = true;
            OnThisEnabled();
        }
        private void OnDisable()
        {
            foreach (var spriteSelectable in _navs)
            {
                spriteSelectable.enabled = false;
            }
            if (!PreserveIndexOnDeselected)
            {
                _index = 0;
            }
            if (_map != null) _map.enabled = false;
            OnThisDisabled();
        }
        protected void OnDestroy()
        {
            _onSelected?.RemoveAllListeners();
        }
        protected virtual void OnThisEnabled() { }
        protected virtual void OnThisDisabled() { }
        protected virtual void SelectOther(SpriteNavigator other, UnityEngine.Events.UnityAction callback = null)
        {
            other.enabled = true;
            callback?.Invoke();
            other.Current.SelectThis();
            enabled = false;
        }
        public void DeselectAll()
        {
            EventSystem.current.SetSelectedGameObject(null, null);
        }
    }
}
