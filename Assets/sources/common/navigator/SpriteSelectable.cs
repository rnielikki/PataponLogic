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

        internal void Init(SpriteNavigator navigator)
        {
            _parent = navigator;
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
            InitCommon(navigator, bg, navigator.PositionOffset, navigator.OnSelected);
        }
        private void InitCommon(SpriteNavigator spriteNavigator, GameObject instantiated, Vector2 positionOffset,
            UnityEvent<SpriteSelectable> onSelected)
        {
            _parent = spriteNavigator;
            instantiated.transform.parent = transform;
            instantiated.transform.position = transform.position + (Vector3)positionOffset;
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
