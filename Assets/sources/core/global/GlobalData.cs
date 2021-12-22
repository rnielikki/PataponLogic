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

        public static MapInfo MapInfo { get; private set; }

        public static GlobalInputSystem GlobalInputActions { get; private set; }

        public static int TipIndex { get; set; }
        // Start is called before the first frame update
        void Awake()
        {
            TipIndex = -1;
            DontDestroyOnLoad(gameObject);
            Input = GetComponent<PlayerInput>();
            GlobalInputActions = new GlobalInputSystem(Input);
            Sound = GetComponentInChildren<GlobalSoundSystem>();

            //---------------------------------- Not serialized.
            PataponInfo = new PataponInfo();

            ItemLoader.LoadAll();
            Inventory = new Inventory(); //must be loaded after item loader init

            MapInfo = new MapInfo();
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
                return default;
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
