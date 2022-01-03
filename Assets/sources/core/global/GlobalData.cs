using PataRoad.Core.Global.Slots;
using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Globally initializes all. LOADED AT VERY FIRST AND NEVER BEEN DESTROYED.
/// </summary>
namespace PataRoad.Core.Global
{
    public class GlobalData : MonoBehaviour
    {
        public static GlobalSoundSystem Sound { get; private set; }
        public static PlayerInput Input { get; private set; }
        public static GlobalInputSystem GlobalInputActions { get; private set; }

        public static int TipIndex { get; set; }
        public static SlotManager SlotManager { get; set; }
        public static Slot CurrentSlot => SlotManager.CurrentSlot;
        /// <summary>
        /// Loads System data.
        /// </summary>
        void Awake()
        {
            TipIndex = -1;
            DontDestroyOnLoad(gameObject);
            Input = GetComponent<PlayerInput>();
            GlobalInputActions = new GlobalInputSystem(Input);
            Sound = GetComponentInChildren<GlobalSoundSystem>();

            ItemLoader.LoadAll();

            SlotManager = new SlotManager();

            //--- test data
            SlotManager.LoadSlot(Slot.LoadSlot(0));
        }
    }
}
