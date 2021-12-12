using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class AttackTypeMenuElement : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Text _text;
        public int Index { get; private set; }
        public void Init(int index, string text, UnityEngine.Events.UnityAction<int> callback)
        {
            Index = index;
            _text.text = text;
            _image.enabled = false;
            _button.onClick.AddListener(() => callback(index));
        }
        public void OnSelect(BaseEventData eventData)
        {
            _image.enabled = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _image.enabled = false;
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
