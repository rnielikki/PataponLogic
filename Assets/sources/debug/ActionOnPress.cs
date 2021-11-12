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
            var loaded = Resources.Load<GameObject>("Characters/Equipments/Weapons/Bird/1");
            var res = loaded.GetComponent<Core.Character.Equipments.EquipmentData>();
            res.LoadImage();
            Core.Items.ItemDrop.DropItem(res, Vector2.zero, 9999999);

            loaded = Resources.Load<GameObject>("Characters/Equipments/Helm/1");
            res = loaded.GetComponent<Core.Character.Equipments.EquipmentData>();
            res.LoadImage();
            Core.Items.ItemDrop.DropItem(res, Vector2.right, 9999999);
            //GetComponent<Core.Character.Patapons.PataponsManager>().Groups.First().Patapons.First().Die();
        }
    }
}
