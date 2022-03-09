using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    /// <summary>
    /// Summary window of Rarepon requirement, that shows automatically when selected.
    /// </summary>
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
        private RectTransform _canvasTransform;
        internal void Init()
        {
            _templates = GetComponentsInChildren<RareponRequirementItem>(true);
            _canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }
        public void ShowRequirements(Core.Items.ItemRequirement[] requirements, RectTransform attachTarget, Vector2 pivot)
        {
            HideRequirements();

            var size = _parentField.sizeDelta;
            size.y = requirements.Length * _heightForUnit;
            _parentField.sizeDelta = size;

            _parentField.anchoredPosition = new Vector3(attachTarget.transform.position.x + 50 - (100 * pivot.x),
                attachTarget.transform.position.y - (_canvasTransform.transform.position.y * 2) + 50 - (100 * pivot.y), 0);
            _parentField.pivot = pivot;

            gameObject.SetActive(true);
            bool available = true;
            for (int i = 0; i < requirements.Length; i++)
            {
                var isAvailable = _templates[i].SetValues(requirements[i], _availableColor, _notAvailableColor);
                available = available && isAvailable;
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