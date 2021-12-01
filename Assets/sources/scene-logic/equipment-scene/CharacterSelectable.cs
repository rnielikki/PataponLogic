using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterSelectable : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler
    {
        private SpriteRenderer _renderer;
        private CharacterNavigator _parent;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _parent = GetComponentInParent<CharacterNavigator>();
        }
        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log("Deselected :( " + gameObject.name);
            _renderer.color = Color.red;
        }

        public void OnMove(AxisEventData eventData)
        {
            Debug.Log("Moving! " + gameObject.name);
            _renderer.color = Color.blue;
        }

        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log("Selected :3 " + gameObject.name);
            _renderer.color = Color.black;
        }
        public void SelectThis() =>
            EventSystem.current.SetSelectedGameObject(gameObject, null);
    }
}
