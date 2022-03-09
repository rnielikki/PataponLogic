using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.Intro
{
    class PenSigning : MonoBehaviour
    {
        private RectTransform _parent;
        private RectTransform _self;
        private InputAction _navigatingAction;
        private InputAction _signAction;
        [SerializeField]
        private SignField _signField;
        [SerializeField]
        private float speed;
        private Vector2 _direction;
        private bool _moving;
        private Canvas _canvas;
        [SerializeField]
        AudioClip _selectedSound;
        [SerializeField]
        UnityEngine.Events.UnityEvent _onSigned;
        [SerializeField]
        UnityEngine.Events.UnityEvent _onStarted;
        [SerializeField]
        Material _skyboxOnNextScene;
        private void Start()
        {
            _parent = transform.parent.GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _self = GetComponent<RectTransform>();
            _navigatingAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Navigate");
            _signAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Submit");
            _navigatingAction.performed += Move;
            _navigatingAction.canceled += StopMoving;
            _signAction.performed += Sign;

            _self.anchoredPosition = new Vector2(_parent.rect.size.x * 0.75f, _parent.rect.size.y * 0.25f);
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _onStarted.Invoke();
        }
        private void Move(InputAction.CallbackContext context)
        {
            _direction = speed * Time.deltaTime * context.ReadValue<Vector2>();
            _moving = true;
        }
        private void StopMoving(InputAction.CallbackContext context)
        {
            _direction = Vector2.zero;
            _moving = false;
        }
        private void Sign(InputAction.CallbackContext context)
        {
            if (_signField.IsOn)
            {
                Core.Global.GlobalData.Sound.PlayInScene(_selectedSound);
                _canvas.renderMode = RenderMode.WorldSpace;
                RenderSettings.skybox = _skyboxOnNextScene;
                DynamicGI.UpdateEnvironment();
                _onSigned.Invoke();
                gameObject.SetActive(false);
            }
        }
        private bool IsOnButton()
        {
            var pos = _signField.RectTransform.rect.center
                + _signField.RectTransform.anchoredPosition + (Vector2.right * _parent.rect.size.x / 2);
            var scale = _signField.RectTransform.rect.size / 2;

            return (_self.anchoredPosition.x > pos.x - scale.x)
            && (_self.anchoredPosition.x < pos.x + scale.x)
            && (_self.anchoredPosition.y > pos.y - scale.y)
            && (_self.anchoredPosition.y < pos.y + scale.y);
        }
        private void Update()
        {
            if (!_moving) return;

            _self.anchoredPosition = new Vector2(
                    Mathf.Clamp(_self.anchoredPosition.x + _direction.x, 0, _parent.rect.size.x),
                    Mathf.Clamp(_self.anchoredPosition.y + _direction.y, 0, _parent.rect.size.y)
                    );
            if (IsOnButton())
            {
                _signField.MarkAsSelected();
            }
            else
            {
                _signField.MarkAsNotSelected();
            }
        }
        private void OnDestroy()
        {
            _navigatingAction.performed -= Move;
            _navigatingAction.canceled -= StopMoving;
            _signAction.performed -= Sign;
        }
    }
}
