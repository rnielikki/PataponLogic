using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// This sometimes won't work due to the inputsystem bug. Let me know when it doesn't work...
    /// </summary>
    public class ConfirmDialog : MonoBehaviour
    {
        private UnityEngine.Events.UnityAction _onConfirmed;
        private UnityEngine.Events.UnityAction _onCancled;
        private MonoBehaviour _targetToResume;

        [SerializeField]
        private Text _content;
        public Text Content => _content;
        [SerializeField]
        private Button _okButton;
        [SerializeField]
        private Button _cancelButton;

        private InputAction _uiOkAction;
        private InputAction _uiCancelAction;
        private GameObject _lastSelected;

        private static GameObject _dialogTemplate { get; set; }
        private const string _path = "Common/Display/Dialog";

        public bool IsScreenChange { get; set; }

        private void Start()
        {
            _uiOkAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Submit");
            _uiCancelAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Cancel");

            //without waiting for next, it just continues... :/
            StartCoroutine(
                Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                _okButton.onClick.AddListener(Confirm);
                _cancelButton.onClick.AddListener(Cancel);
                _uiCancelAction.performed += Cancel;
            }));
        }
        public static ConfirmDialog Create(string content)
        {
            if (_dialogTemplate == null) _dialogTemplate = Resources.Load<GameObject>(_path);
            var dialog = Instantiate(_dialogTemplate).GetComponent<ConfirmDialog>();
            dialog._okButton.Select();
            dialog._content.text = content;
            return dialog;
        }
        public ConfirmDialog SetTargetToResume(MonoBehaviour targetToResume)
        {
            var lastSelected = DisableParentAndGetLastSelection(targetToResume);
            InitParent(targetToResume, lastSelected);
            return this;
        }

        public ConfirmDialog SelectCancel()
        {
            _cancelButton.Select();
            return this;
        }
        public ConfirmDialog SetOkAction(UnityEngine.Events.UnityAction onConfirmed)
        {
            _onConfirmed = onConfirmed;
            return this;
        }

        public ConfirmDialog SetCancelAction(UnityEngine.Events.UnityAction onCanceled)
        {
            _onCancled = onCanceled;
            return this;
        }
        public ConfirmDialog HideOkButton()
        {
            _okButton.gameObject.SetActive(false);
            _cancelButton.Select();
            return this;
        }
        private static GameObject DisableParentAndGetLastSelection(MonoBehaviour targetToResume)
        {
            var actionMap = targetToResume.GetComponent<Navigator.ActionEventMap>();
            if (actionMap != null) actionMap.enabled = false;
            targetToResume.enabled = false;
            return UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        }
        public ConfirmDialog InitParent(MonoBehaviour targetToResume, GameObject lastSelected)
        {
            _targetToResume = targetToResume;
            _lastSelected = lastSelected;
            return this;
        }
        public void Confirm()
        {
            _onConfirmed?.Invoke();
            Close(true);
        }
        public void Cancel(InputAction.CallbackContext context) => Cancel();
        public void Cancel()
        {
            _onCancled?.Invoke();
            Close(false);
        }
        private void Close(bool ok)
        {
            _uiOkAction.Disable();
            _uiCancelAction.Disable();

            Core.Global.GlobalData.Sound.PlaySelected();

            if (_targetToResume != null)
            {
                _targetToResume.enabled = true;
                var actionMap = _targetToResume.GetComponent<Navigator.ActionEventMap>();
                if (actionMap != null) actionMap.enabled = true;
            }
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_lastSelected);

            if (!IsScreenChange || !ok)
            {
                _uiOkAction.Enable();
                _uiCancelAction.Enable();
            }
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            _uiCancelAction.performed -= Cancel;
        }
    }
}
