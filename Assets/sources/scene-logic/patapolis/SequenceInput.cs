using PataRoad.Core.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.Patapolis
{
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
            _sequence[0].performed += StartListening;
        }
        void StartListening(InputAction.CallbackContext context)
        {
            var action = context.action;
            action.performed -= StartListening;

            if (action == _sequence[_sequenceIndex])
            {
                _sequenceIndex++;
                if (_sequenceIndex >= _sequence.Length)
                {
                    CallInputAction();
                }
                else
                {
                    _sequence[_sequenceIndex].performed += StartListening;
                }
            }
            else
            {
                _sequenceIndex = 0;
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
        }
    }
}