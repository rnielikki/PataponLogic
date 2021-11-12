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
            IItem item1 = ItemLoader.Load(ItemType.Equipment, "Bird", 1);
            IItem item2 = ItemLoader.Load(ItemType.Equipment, "Helm", 1);

            ItemDrop.DropItem(item1, Vector2.zero, 9999999);
            ItemDrop.DropItem(item2, Vector2.right, 9999999);
        }
    }
}
