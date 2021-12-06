using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PataRoad.Common.Navigator
{
    /// <summary>
    /// One key-value pair of action and event map, which is serializable and visible to editor. Use inside <see cref="ActionEventMap"/>.
    /// </summary>
    [System.Serializable]
    public class ActionEventPair
    {
        InputAction _action;
        UnityEvent<Object, InputAction.CallbackContext> _senderEvent;
        private Object _sender;
        private bool _addedEvent;
        public ActionEventPair(string fullActionName, Object sender, UnityEvent<Object, InputAction.CallbackContext> senderEvent)
        {
            _action = Core.GlobalData.Input.actions.FindAction(fullActionName);
            _sender = sender;
            _senderEvent = senderEvent;
            _action.performed += InvokeEvent;
            _action.Enable();
            _addedEvent = true;
        }
        public void Activate()
        {
            if (!_addedEvent)
            {
                _action.performed += InvokeEvent;
                _addedEvent = true;
            }
        }
        public void Deactivate()
        {
            if (_addedEvent)
            {
                _action.performed -= InvokeEvent;
                _addedEvent = false;
            }
        }
        private void InvokeEvent(InputAction.CallbackContext callbackContext)
        {
            if (!UnityEngine.EventSystems.EventSystem.current.alreadySelecting)
            {
                try
                {
                    _senderEvent.Invoke(_sender, callbackContext);
                }
                catch (System.Exception)
                {
                    Debug.LogError($"Error while sending {_senderEvent.GetPersistentEventCount()} of events with sender {_sender}.");
                    throw;
                }
            }
        }

        internal void SetSender(Object sender)
        {
            _sender = sender;
        }

        public void Destroy()
        {
            _action.performed -= InvokeEvent;
            _action.Disable();
            _senderEvent.RemoveAllListeners();
        }
    }
}
