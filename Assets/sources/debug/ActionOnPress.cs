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
            FindObjectOfType<Core.Rhythm.Command.MiracleListener>().OnMiracle.AddListener(() => Debug.Log("------------------- MIRACLE ---------------------"));

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
            FirstPon.StatusEffectManager.SetFire(4);
        }
        public void SetIce()
        {
            FirstPon.StatusEffectManager.SetIce(4);
        }
        public void SetSleep()
        {
            FirstPon.StatusEffectManager.SetSleep(4);
        }
        public void SetStagger()
        {
            FirstPon.StatusEffectManager.SetStagger();
        }
        public void SetKnockback()
        {
            FirstPon.StatusEffectManager.SetKnockback();
        }

        public void DropItem()
        {
        }
    }
}
