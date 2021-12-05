using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PataRoad.Common.Navigator
{
    public class SpriteSelectable : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler
    {
        private SpriteNavigator _parent;
        private SpriteRenderer _renderer;
        private UnityEvent<SpriteSelectable> _onSelected;

        private bool _freeze; //set as deselcted but looks like selected.
        private bool _firstInit = true;

        internal void Init(SpriteNavigator navigator)
        {
            _parent = navigator;
            if (_firstInit)
            {
                GameObject bg;
                if (navigator.UseSprite)
                {
                    var background = navigator.Background;
                    bg = new GameObject("selectingSprite");
                    _renderer = bg.AddComponent<SpriteRenderer>();
                    _renderer.sprite = background;

                }
                else
                {
                    var backgroundObject = navigator.BackgroundObject;
                    bg = Instantiate(backgroundObject);
                    _renderer = bg.GetComponentInChildren<SpriteRenderer>();
                }
                bg.transform.parent = transform;
                bg.transform.position = transform.position + (Vector3)navigator.PositionOffset;
                _firstInit = false;
            }
            InitCommon(navigator, navigator.OnSelected);

        }
        private void InitCommon(SpriteNavigator spriteNavigator, UnityEvent<SpriteSelectable> onSelected)
        {
            _parent = spriteNavigator;
            _renderer.enabled = false;
            _onSelected = onSelected;
        }
        public void Freeze()
        {
            _freeze = true;
            EventSystem.current.SetSelectedGameObject(null, null);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if (!_freeze) _renderer.enabled = false;
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.moveDir == MoveDirection.Left || eventData.moveDir == MoveDirection.Right)
            {
                _renderer.enabled = false;
                _parent.MoveTo(eventData.moveDir);
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            _renderer.enabled = true;
            _onSelected?.Invoke(this);

            _freeze = false;
        }
        public void SelectThis()
        {
            EventSystem.current.SetSelectedGameObject(gameObject, null);
        }
    }
}
