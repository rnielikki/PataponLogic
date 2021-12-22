using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    public class SummaryElement : MonoBehaviour
    {
        [SerializeField]
        protected Color _selectedColor;
        protected Color _notSelectedColor;

        protected Image _bg;
        protected Text _text;

        public virtual void Init()
        {
            _bg = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            _notSelectedColor = _bg.color;
        }
        public void MarkAsDeselected()
        {
            _bg.color = _notSelectedColor;
        }
        public void MarkAsSelected()
        {
            _bg.color = _selectedColor;
        }
    }
}