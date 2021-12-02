using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PataRoad.Common.Navigator
{
    /// <summary>
    /// One key-value pair of action and event map, which is serializable and visible to editor. Use inside <see cref="SpriteActionMap"/>.
    /// </summary>
    [System.Serializable]
    public class ActionEventPair
    {
        InputAction _action;
        UnityEvent<Object> _senderEvent;
        private Object _sender;
        private bool _addedEvent;
        public ActionEventPair(string fullActionName, Object sender, UnityEvent<Object> senderEvent)
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
                _senderEvent.Invoke(_sender);
            }
        }

        internal void SetSender(Object sender)
        {
            _sender = sender;
        }

        public void Destroy()
        {
            _action.performed -= InvokeEvent;
            _senderEvent.RemoveAllListeners();
        }
    }
}
