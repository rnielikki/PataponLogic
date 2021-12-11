using System.Collections;
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
        [SerializeField]
        private AudioClip _buttonClickSound;

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
            _uiOkAction.Disable();
            _uiCancelAction.Disable();

            //without waiting for next, it just continues... :/
            StartCoroutine(WaitForNext());
            IEnumerator WaitForNext()
            {
                if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate)
                {
                    yield return new WaitForEndOfFrame();
                }
                else if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInFixedUpdate)
                {
                    yield return new WaitForFixedUpdate();
                }
                _okButton.onClick.AddListener(Confirm);
                _cancelButton.onClick.AddListener(Cancel);
                _uiCancelAction.performed += Cancel;

                _uiOkAction.Enable();
                _uiCancelAction.Enable();
            }
        }

        public static ConfirmDialog Create(string text, MonoBehaviour targetToResume, UnityEngine.Events.UnityAction onConfirmed,
            UnityEngine.Events.UnityAction onCanceled = null, bool okAsDefault = true)
        {
            var lastSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            var actionMap = targetToResume.GetComponent<Navigator.ActionEventMap>();
            if (actionMap != null) actionMap.enabled = false;

            var dialog = Create(text, onConfirmed, onCanceled, okAsDefault);
            dialog._targetToResume = targetToResume;
            dialog._lastSelected = lastSelected;
            targetToResume.enabled = false;
            return dialog;
        }
        public static ConfirmDialog Create(string text, UnityEngine.Events.UnityAction onConfirmed, UnityEngine.Events.UnityAction onCanceled = null, bool okAsDefault = true)
        {
            if (_dialogTemplate == null) _dialogTemplate = Resources.Load<GameObject>(_path);
            var dialog = Instantiate(_dialogTemplate).GetComponent<ConfirmDialog>();
            dialog._onConfirmed = onConfirmed;
            dialog._onCancled = onCanceled;
            dialog._content.text = text;
            if (okAsDefault)
            {
                dialog._okButton.Select();
            }
            else
            {
                dialog._cancelButton.Select();
            }
            return dialog;
        }
        public static ConfirmDialog CreateCancelOnly(string text, UnityEngine.Events.UnityAction onCanceled = null)
        {
            var dialog = Create(text, null, onCanceled, false);
            dialog._okButton.gameObject.SetActive(false);
            return dialog;
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

            Core.Global.GlobalData.Sound.Play(_buttonClickSound);

            if (_targetToResume != null) _targetToResume.enabled = true;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_lastSelected);
            var actionMap = _targetToResume.GetComponent<Navigator.ActionEventMap>();
            if (actionMap != null) actionMap.enabled = true;

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
