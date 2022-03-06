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
            //FirstPon.StatusEffectManager.SetKnockback();
            //Enemy.StatusEffectManager.SetKnockback();
            FindObjectOfType<Core.Character.Bosses.Boss>().StatusEffectManager.SetKnockback();
        }
        public void LoadStory()
        {
            Story.StoryLoader.Init();
            Story.StoryLoader.LoadStory(_testData);
        }
        public void OpenAllMaps()
        {
            /*
            var mapInfo = Core.Global.GlobalData.CurrentSlot.MapInfo;
            for (int i = 0; i < mapInfo.Progress; i++)
            {
                mapInfo.OpenInIndex(i);
            }
            */
        }
        public void SuccessMission()
        {
            var current = Core.Map.MissionPoint.Current;
            if (current == null) return;
            current.FilledMissionCondition = true;
            current.EndMission();
        }
        public void AddClass()
        {
            //Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Class", 4));
            //Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Song", 4));
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Song", 3));
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Song", 4));
            Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Key, "Song", 5));
        }
        public void AddAllMaterials()
        {
            string[] materials = new string[] { "Alloy", "Bone", "Fang", "Hide", "Liquid", "Meat", "Mineral", "Seed", "Tree", "Vegetable" };
            foreach (var material in materials)
            {
                for (int i = 0; i < 5; i++)
                {
                    Core.Global.GlobalData.CurrentSlot.Inventory.AddMultiple(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Material, material, i), 99);
                }
            }
            Core.Global.GlobalData.CurrentSlot.Inventory.AddMultiple(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Equipment, "Gem", 1), 99);
            Core.Global.GlobalData.CurrentSlot.Inventory.AddMultiple(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Equipment, "Gem", 2), 99);
            Core.Global.GlobalData.CurrentSlot.Inventory.AddMultiple(Core.Items.ItemLoader.GetItem(Core.Items.ItemType.Equipment, "Gem", 3), 99);
        }
    }
}
