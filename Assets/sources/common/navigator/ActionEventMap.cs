using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.Common.Navigator
{
    public class ActionEventMap : MonoBehaviour
    {
#pragma warning disable S1104 // Fields should not have public accessibility. Shut up if you cannot fix editor. Even serializereference does nothing.
        [System.Serializable]
        public class AEPair
        {
            public string ActionName;
            public UnityEngine.Events.UnityEvent<Object, InputAction.CallbackContext> OnPerformed;
        }
        public AEPair[] _actionAndEvents;
#pragma warning restore S1104 // Fields should not have public accessibility
        private readonly System.Collections.Generic.List<ActionEventPair> _pairs = new System.Collections.Generic.List<ActionEventPair>();
        [SerializeField]
        Object _sender;

        private void Start()
        {
            if (_sender == null) _sender = gameObject;

            foreach (var pair in _actionAndEvents)
            {
                var actionEventPair = new ActionEventPair(pair.ActionName, _sender, pair.OnPerformed);
                _pairs.Add(actionEventPair);
                actionEventPair.Activate();
            }
        }
        public void SetSender(Object sender)
        {
            _sender = sender;
            foreach (var pair in _pairs)
            {
                pair.SetSender(_sender);
            }
        }
        private void OnEnable()
        {
            foreach (var pair in _pairs)
            {
                pair.Activate();
            }
        }
        private void OnDisable()
        {
            foreach (var pair in _pairs)
            {
                pair.Deactivate();
            }
        }
        private void OnDestroy()
        {
            foreach (var pair in _pairs)
            {
                pair.Destroy();
            }
        }
    }
}
