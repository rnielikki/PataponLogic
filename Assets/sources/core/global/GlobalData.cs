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

        public static SlotManager SlotManager { get; set; }
        public static Slot CurrentSlot => SlotManager.CurrentSlot;

        public static Settings.SettingModel Settings { get; private set; }

        [SerializeField]
        InputActionAsset _leftInputs;
        [SerializeField]
        InputActionAsset _rightInputs;

        [Header("Debug")]
        [SerializeField]
        bool _doNotLoadMain;


        /// <summary>
        /// Loads System data.
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            try
            {
                Settings = Global.Settings.SettingModel.Load();
                ItemLoader.LoadAll();
                Character.Patapons.Data.RareponInfo.LoadAll();
                Common.GameDisplay.TipsCollection.LoadAllTips();

                Input = GetComponent<PlayerInput>();
                GlobalInputActions = new GlobalInputSystem(Input, _leftInputs, _rightInputs);
                Sound = GetComponentInChildren<GlobalSoundSystem>();

                SlotManager = new SlotManager();
                if (!_doNotLoadMain) UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            }
            catch (System.Exception e)
            {
                var textField = FindObjectOfType<TMPro.TextMeshProUGUI>();
                textField.text = "Failed to load System data with error:"
                    + $"\n{e.Message}"
                    + "\nPress Alt+F4 to close window.";
                throw;
            }
        }
    }
}
