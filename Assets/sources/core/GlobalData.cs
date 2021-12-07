using PataRoad.Core.Character.Patapons.Data;
using PataRoad.Core.Items;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Globally initializes all. LOADED AT VERY FIRST AND NEVER BEEN DESTROYED.
/// </summary>
namespace PataRoad.Core
{
    public class GlobalData : MonoBehaviour
    {
        public static PlayerInput Input { get; private set; }
        public static PataponInfo PataponInfo { get; } = new PataponInfo();
        public static Inventory Inventory { get; private set; }
        [SerializeField]
        int _tipIndex = -1;
        public int TipIndex => _tipIndex;
        public static AudioSource GlobalAudioSource { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GlobalAudioSource = GetComponentInChildren<AudioSource>();
            Input = GetComponent<PlayerInput>();

            ItemLoader.LoadAll();
            Inventory = new Inventory(); //must be loaded after item loader init
        }
        public static bool TryGetActionBindingName(string actionName, out string name)
        {
            //Note: To make GetBindingDisplayString() work, you MUST select CORRECT BINDING TYPE (keyboard, gamepad...) in input system.
            var actionBindingName = Input.actions.FindAction(actionName)?.GetBindingDisplayString();
            name = actionBindingName;
            return !string.IsNullOrEmpty(actionBindingName);
        }
    }
}
