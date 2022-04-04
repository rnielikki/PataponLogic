using PataRoad.Core.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.Patapolis
{
    /// <summary>
    /// Currently not working.
    /// </summary>
    public class SequenceInput : MonoBehaviour
    {
        [SerializeField]
        string[] _inputActions;
        InputAction[] _sequence;
        private int _sequenceIndex;
        void Start()
        {
            _sequence = new InputAction[_inputActions.Length];
            for (int i = 0; i < _sequence.Length; i++)
            {
                _sequence[i] = GlobalData.Input.actions
                    .FindAction(_inputActions[i]);
            }
            ResetListening();
        }
        void ResetListening()
        {
            _sequenceIndex = 0;
            _sequence[0].performed += StartListening;
            GlobalData.Input.onActionTriggered -= ListenInput;
        }
        void StartListening(InputAction.CallbackContext context)
        {
            _sequence[0].performed -= StartListening;
            GlobalData.Input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            GlobalData.Input.onActionTriggered += ListenInput;
        }
        void ListenInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (FindFromArray())
            {
                _sequenceIndex++;
                if (_sequenceIndex >= _sequence.Length)
                {
                    CallInputAction();
                    GlobalData.Input.onActionTriggered -= ListenInput;
                }
            }
            else
            {
                ResetListening();
            }
            bool FindFromArray()
            {
                foreach (var ctrl in _sequence[_sequenceIndex].controls)
                {
                    if (ctrl == context.action.activeControl)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        private void CallInputAction()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("lol");
            _sequenceIndex = 0;
        }
        private void OnDestroy()
        {
            _sequence[_sequenceIndex].performed -= StartListening;
            GlobalData.Input.onActionTriggered -= ListenInput;
        }
    }
}