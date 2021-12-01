using PataRoad.Core.Character;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterNavigator : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler
    {
        private CharacterSelectable _currentSelectableCharacter;
        private List<CharacterSelectable> _selectables = new List<CharacterSelectable>();
        CharacterGroupNavigator _parent;
        private SpriteRenderer _renderer;
        private int _index;

        public void Init(CharacterGroupNavigator parent, Sprite groupBackground, GameObject characterBackground)
        {
            _parent = parent;
            _renderer = gameObject.AddComponent<SpriteRenderer>();
            _renderer.sprite = groupBackground;
            foreach (var patapon in GetComponentsInChildren<PataponData>())
            {
                _selectables.Add(Instantiate(characterBackground, patapon.transform).GetComponent<CharacterSelectable>());
            }
            _currentSelectableCharacter = _selectables[0];
            //_currentSelectableCharacter.SelectThis();
        }
        public void OnDeselect(BaseEventData eventData)
        {
            _renderer.color = Color.white;
        }

        public void OnMove(AxisEventData eventData)
        {
            _parent.MoveTo(eventData.moveDir);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _renderer.color = Color.blue;
        }
        public void SelectThis() =>
            EventSystem.current.SetSelectedGameObject(gameObject, null);

        public void MoveTo(MoveDirection direction)
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
            _currentSelectableCharacter = _selectables[_index];
            _currentSelectableCharacter.SelectThis();
        }
    }
}
