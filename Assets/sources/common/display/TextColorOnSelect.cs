namespace PataRoad.Common.GameDisplay
{
    public class TextColorOnSelect : UnityEngine.MonoBehaviour, UnityEngine.EventSystems.ISelectHandler, UnityEngine.EventSystems.IDeselectHandler
    {
        [UnityEngine.SerializeField]
        private UnityEngine.UI.Text _text;
        [UnityEngine.SerializeField]
        private UnityEngine.Color _colorOnSelect;
        private UnityEngine.Color _colorOnDeselect;
        private void Awake()
        {
            _colorOnDeselect = _text.color;
        }
        private void OnDisable()
        {
            _text.color = _colorOnDeselect;
        }
        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            _text.color = _colorOnSelect;
        }
        public void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            _text.color = _colorOnDeselect;
        }
    }
}
