using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    internal class RareponRequirementWindow : MonoBehaviour
    {
        [SerializeField]
        private RareponRequirementItem[] _templates;
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private RectTransform _parentField;
        [SerializeField]
        private Color _notAvailableColor;
        [SerializeField]
        private Color _availableColor;
        [SerializeField]
        private Color _notAvailableBackgroundColor;
        [SerializeField]
        private int _heightForUnit;
        internal void Init()
        {
            _templates = GetComponentsInChildren<RareponRequirementItem>();
        }
        public void ShowRequirements(Core.Items.ItemRequirement[] requirements, RectTransform attachTarget, Vector2 pivot)
        {
            HideRequirements();

            var size = _parentField.sizeDelta;
            size.y = requirements.Length * _heightForUnit;
            _parentField.sizeDelta = size;

            _parentField.localPosition = attachTarget.localPosition;
            _parentField.pivot = pivot;

            gameObject.SetActive(true);
            bool available = true;
            for (int i = 0; i < requirements.Length; i++)
            {
                available = available && _templates[i].SetValues(requirements[i], _availableColor, _notAvailableColor);
            }
            if (!available) _backgroundImage.color = _notAvailableBackgroundColor;
        }
        public void HideRequirements()
        {
            for (int i = 0; i < _templates.Length; i++)
            {
                var value = _templates[i];
                if (value.gameObject.activeSelf)
                {
                    value.Hide();
                }
                else break;
            }
            gameObject.SetActive(false);
        }
    }
}