using PataRoad.Core.Character.Patapons.Data;
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
        public static PataponInfo PataponInfo { get; private set; }
        public static Inventory Inventory { get; private set; }

        public static Map.MapData MapData { get; set; }

        [SerializeField]
        int _tipIndex = -1;
        public int TipIndex => _tipIndex;
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Input = GetComponent<PlayerInput>();
            Sound = GetComponentInChildren<GlobalSoundSystem>();
            MapData = new Map.MapData();

            PataponInfo = new PataponInfo();

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
        public void Serialize(string key, IPlayerData data)
        {
            var str = data.Serialize();
            PlayerPrefs.SetString(key, str);
        }
        public T Deserialize<T>(string key) where T : IPlayerData
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return default(T);
            }
            else
            {
                try
                {
                    var data = PlayerPrefs.GetString(key);
                    var res = JsonUtility.FromJson<T>(data);
                    res.Deserialize();
                    return res;
                }
                catch (System.Exception)
                {
                    Debug.LogError("Error while loading item. loading default items...");
                    return default(T);
                }
            }
        }
    }
}
