using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Character
{
    class HealthDisplay : MonoBehaviour
    {
        private RectTransform _statusBar;
        [SerializeField]
        private Gradient _colorOverHealth;
        private Image _statusImage;

        [Tooltip("Don't register on Patapon status bar.")]
        private ICharacter _target;
        private void Awake()
        {
            _statusBar = GetComponent<RectTransform>();
            _statusImage = GetComponent<Image>();
            _statusImage.color = _colorOverHealth.Evaluate(1);
            _target?.OnDamageTaken?.AddListener(UpdateBar);
        }
        public void UpdateBar(float ratio)
        {
            _statusBar.anchorMax = new Vector2(ratio, 1);
            _statusImage.color = _colorOverHealth.Evaluate(ratio);
        }
    }
}
