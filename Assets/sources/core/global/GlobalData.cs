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

        public static Settings.SettingModel Settings { get; private set; }

        /// <summary>
        /// Loads System data.
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Settings = Global.Settings.SettingModel.Load();
            ItemLoader.LoadAll();
            Character.Patapons.Data.RareponInfo.LoadAll();

            TipIndex = -1;
            Input = GetComponent<PlayerInput>();
            GlobalInputActions = new GlobalInputSystem(Input);
            Sound = GetComponentInChildren<GlobalSoundSystem>();

            SlotManager = new SlotManager();
        }
    }
}
