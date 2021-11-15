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
        private Core.Character.Patapons.Patapon FirstPon => GetComponent<Core.Character.Patapons.PataponsManager>().Groups.First().Patapons.First();
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
            FirstPon.Die();
        }
        public void SetFire()
        {
            FirstPon.StatusEffectManager.SetFire(10);
        }
        public void SetSleep()
        {
            FirstPon.StatusEffectManager.SetSleep(10);
        }

        public void DropItem()
        {
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Staff", 0), Vector2.zero, 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Bird", 1), new Vector2(-0.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Helm", 0), new Vector2(-1, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Helm", 1), new Vector2(-1.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Bow", 0), new Vector2(-2, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Lance", 0), new Vector2(-2.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Spear", 1), new Vector2(-3, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Javelin", 0), new Vector2(-3.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Horse", 0), new Vector2(-4, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Horn", 0), new Vector2(-4.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Staff", 0), new Vector2(1, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Staff", 1), new Vector2(0.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Sword", 0), new Vector2(2, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Cape", 0), new Vector2(2.5f, 0), 999);
            ItemDrop.DropItem(ItemLoader.GetItem(ItemType.Equipment, "Shield", 0), new Vector2(3, 0), 999);
        }
    }
}
