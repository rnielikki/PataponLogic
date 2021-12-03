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
        private UnityEvent<SpriteSelectable> _onDeselected;

        private bool _freeze; //set as deselcted but looks like selected.

        internal void Init(SpriteNavigator spriteNavigator, Sprite background, Vector2 positionOffset,
            UnityEvent<SpriteSelectable> onSelected, UnityEvent<SpriteSelectable> onDeselected = null)
        {
            var bg = new GameObject("selectingSprite");
            _renderer = bg.AddComponent<SpriteRenderer>();
            _renderer.sprite = background;
            InitCommon(spriteNavigator, bg, positionOffset, onSelected, onDeselected);
        }
        internal void Init(SpriteNavigator spriteNavigator, GameObject backgroundObject, Vector2 positionOffset,
            UnityEvent<SpriteSelectable> onSelected, UnityEvent<SpriteSelectable> onDeselected = null)
        {
            var bg = Instantiate(backgroundObject);
            _renderer = bg.GetComponentInChildren<SpriteRenderer>();
            InitCommon(spriteNavigator, bg, positionOffset, onSelected, onDeselected);
        }
        private void InitCommon(SpriteNavigator spriteNavigator, GameObject instantiated, Vector2 positionOffset,
            UnityEvent<SpriteSelectable> onSelected, UnityEvent<SpriteSelectable> onDeselected = null)
        {
            _parent = spriteNavigator;
            instantiated.transform.parent = transform;
            instantiated.transform.position = transform.position + (Vector3)positionOffset;
            _renderer.enabled = false;
            _onSelected = onSelected;
            _onDeselected = onDeselected;

        }
        public void Freeze()
        {
            _freeze = true;
            EventSystem.current.SetSelectedGameObject(null, null);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if (!_freeze) _renderer.enabled = false;
            _onDeselected?.Invoke(this);
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
