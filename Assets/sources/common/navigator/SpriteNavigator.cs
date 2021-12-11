using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.Common.Navigator
{
    public class SpriteNavigator : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _backgroundObject;
        public GameObject BackgroundObject => _backgroundObject;
        [SerializeField]
        protected bool _useSprite;
        public bool UseSprite => _useSprite;
        [SerializeField]
        protected Sprite _background;
        public Sprite Background => _background;
        [SerializeField]
        protected AudioClip _selectSound;
        public AudioClip SelectSound => _selectSound;
        [SerializeField]
        protected AudioSource _audioSource;
        [SerializeField]
        protected UnityEngine.Events.UnityEvent<SpriteSelectable> _onSelected = new UnityEngine.Events.UnityEvent<SpriteSelectable>();
        public UnityEngine.Events.UnityEvent<SpriteSelectable> OnSelected => _onSelected;
        [SerializeField]
        protected Vector2 _positionOffset;
        public Vector2 PositionOffset => _positionOffset;
        [SerializeField]
        protected bool _preserveIndexOnDeselected;
        public bool PreserveIndexOnDeselected { get; set; }

        protected ActionEventMap _map;

        public SpriteSelectable Current => _selectables[_index];
        protected List<SpriteSelectable> _selectables = new List<SpriteSelectable>();
        protected int _index;
        // Start is called before the first frame update
        protected bool _selectOnInit = true;
        public virtual void Init()
        {
            PreserveIndexOnDeselected = _preserveIndexOnDeselected;
            _selectables.Clear();
            foreach (Transform child in transform)
            {
                var comp = child.gameObject.GetComponent<SpriteSelectable>();
                if (comp == null) continue;
                comp.Init(this);
                _selectables.Add(comp);
            }
            if (_map == null) _map = GetComponent<ActionEventMap>();
            if (_selectOnInit) Current.SelectThis();
        }

        /// <summary>
        /// Show as selected, even it's really deselected. This includes deselecting and disabling the navigator.
        /// </summary>
        public void Freeze()
        {
            var index = _index;
            Current.Freeze();
            enabled = false;
            _index = index;
        }
        public void Defrost()
        {
            enabled = true;
            Current.Defrost();
        }

        public virtual void MoveTo(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                    _index = (_index + 1) % _selectables.Count;
                    break;
                case MoveDirection.Right:
                    _index = (_index - 1 + _selectables.Count) % _selectables.Count;
                    break;
                default:
                    return;
            }
            Current.SelectThis();
            if (_selectSound != null) _audioSource.PlayOneShot(_selectSound);
        }
        private void OnEnable()
        {
            foreach (var spriteSelectable in _selectables)
            {
                spriteSelectable.enabled = true;
            }
            if (_map != null) _map.enabled = true;
            OnThisEnabled();
        }
        private void OnDisable()
        {
            foreach (var spriteSelectable in _selectables)
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
