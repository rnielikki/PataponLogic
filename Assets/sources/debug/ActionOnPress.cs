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
        Story.StoryData _testData;

        [SerializeField]
        private InputAction _input;
        [SerializeField]
        private UnityEvent _action;
        private Core.Character.Patapons.Patapon FirstPon => FindObjectOfType<Core.Character.Patapons.PataponsManager>().FirstPatapon;
        private Core.Character.ICharacter Enemy => FindObjectOfType<Core.Character.Hazorons.Hazoron>();
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
            Enemy.StatusEffectManager.SetFire(4);
        }
        public void SetIce()
        {
            Enemy.StatusEffectManager.SetIce(4);
        }
        public void SetSleep()
        {
            Enemy.StatusEffectManager.SetSleep(4);
        }
        public void SetStagger()
        {
            Enemy.StatusEffectManager.SetStagger();
        }
        public void SetKnockback()
        {
            Debug.Log("knockback!");
            FirstPon.StatusEffectManager.SetKnockback();
            //Enemy.StatusEffectManager.SetKnockback();
        }
        public void LoadStory()
        {
            Story.StoryLoader.Init();
            Story.StoryLoader.LoadStory(_testData);
        }
        public void AddClass()
        {
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Class", 4));
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Song", 4));
        }
    }
}
