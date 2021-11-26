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
        private Core.Character.Bosses.EnemyBoss Boss;
        // Start is called before the first frame update
        void Awake()
        {
            _input.performed += PerformAction;
            _input.Enable();
            Boss = FindObjectOfType<Core.Character.Bosses.EnemyBoss>();
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
            Boss.StatusEffectManager.SetFire(4);
        }
        public void SetIce()
        {
            Boss.StatusEffectManager.SetIce(4);
        }
        public void SetSleep()
        {
            Boss.StatusEffectManager.SetSleep(4);
        }
        public void SetStagger()
        {
            Boss.StatusEffectManager.SetStagger();
        }
        public void SetKnockback()
        {
            Boss.StatusEffectManager.SetKnockback();
        }
    }
}
