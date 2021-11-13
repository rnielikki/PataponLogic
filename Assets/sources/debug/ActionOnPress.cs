using PataRoad.Core.Items;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace PataRoad.AppDebug
{
    public class ActionOnPress : MonoBehaviour
    {
        [SerializeField]
        private InputAction _input;
        [SerializeField]
        private UnityEvent _action;
        // Start is called before the first frame update
        void Awake()
        {
            _input.performed += PerformAction;
            _input.Enable();
        }
        private void OnDestroy()
        {
            _input.performed -= PerformAction;
            _input.Disable();
            _action.RemoveAllListeners();
        }
        private void PerformAction(CallbackContext _context) => _action.Invoke();
        public void KillFirst()
        {
            GetComponent<Core.Character.Patapons.PataponsManager>().Groups.First().Patapons.First().Die();
        }
        public void DropItem()
        {
        }
    }
}
