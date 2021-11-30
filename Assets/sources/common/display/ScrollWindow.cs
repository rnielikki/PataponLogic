using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// Scrolls with the input system setting. Works OnLY with VERTICAL.
    /// </summary>
    public class ScrollWindow : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _viewport;
        [SerializeField]
        private RectTransform _content;
        [SerializeField]
        private float _sensitivity;
        [SerializeField]
        private UnityEngine.UI.Image _upArrow;
        [SerializeField]
        private UnityEngine.UI.Image _downArrow;
        private InputAction _action;
        // Start is called before the first frame update
        void Awake()
        {
            _action = Core.GlobalData.Input.actions.FindAction("UI/Navigate");
        }
        private void OnEnable()
        {
            _action.Enable();
            _action.performed += Scroll;
        }
        private void OnDisable()
        {
            _action.Disable();
            _action.performed -= Scroll;
        }
        private void OnDestroy()
        {
            _action.Disable();
            _action.performed -= Scroll;
        }
        private void Scroll(InputAction.CallbackContext context)
        {
            float maxScroll = _content.rect.size.y - _viewport.rect.size.y;
            if (maxScroll <= 0)
            {
                _downArrow.enabled = false;
                return;
            }

            var pos = _content.anchoredPosition;
            pos.y = Mathf.Clamp(pos.y - _viewport.rect.size.y * _sensitivity * context.ReadValue<Vector2>().y, 0, maxScroll);

            _content.anchoredPosition = pos;
            _upArrow.enabled = pos.y != 0;
            _downArrow.enabled = pos.y != maxScroll;
        }
        private void OnValidate()
        {
            if (_sensitivity <= 0 || _sensitivity >= 1)
            {
                Debug.LogError("Sensitivity must be more than zero and less than one.");
            }
        }
    }
}
