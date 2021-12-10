using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// VERY IMPORTANT NOTE: CURRENTLY IT'S NOT WORKING PROPERLY WITH PARENT WITH "START" INPUT.
    /// PARENT MUST USE "PERFORMED" INPUT (OR NOTHING)
    /// ...input system is annoying.
    /// </summary>
    public class ConfirmDialog : MonoBehaviour
    {
        private UnityEngine.Events.UnityAction _onConfirmed;
        private UnityEngine.Events.UnityAction _onCancled;
        private MonoBehaviour _targetToResume;

        [SerializeField]
        private Text _content;
        [SerializeField]
        private Button _okButton;
        [SerializeField]
        private Button _cancelButton;

        private InputAction _uiOkAction;
        private InputAction _uiCancelAction;
        private GameObject _lastSelected;

        private static GameObject _dialogTemplate { get; set; }
        private const string _path = "Common/Display/Dialog";


        private void Start()
        {
            _uiOkAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Submit");
            _uiCancelAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Cancel");

            _uiOkAction.started += OkOrCancel;
            _uiCancelAction.started += Cancel;
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
        private void OkOrCancel(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == _okButton.gameObject)
            {
                Confirm(context);
            }
            else
            {
                Cancel(context);
            }
        }
        public void Confirm(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            _onConfirmed?.Invoke();
            Close();
        }
        public void Cancel(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            _onCancled?.Invoke();
            Close();
        }
        private void Close()
        {
            _uiOkAction.Disable();
            _uiCancelAction.Disable();
            StartCoroutine(DestroyOnNextFrame());

            IEnumerator DestroyOnNextFrame()
            {
                yield return new WaitForFixedUpdate();
                if (_targetToResume != null) _targetToResume.enabled = true;
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_lastSelected);
                var actionMap = _targetToResume.GetComponent<Navigator.ActionEventMap>();
                if (actionMap != null) actionMap.enabled = true;

                _uiOkAction.Enable();
                _uiCancelAction.Enable();
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            _uiOkAction.started -= OkOrCancel;
            _uiCancelAction.started -= Cancel;
        }

    }
}
