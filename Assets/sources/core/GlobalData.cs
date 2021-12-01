using PataRoad.Core.Character.Patapons.Data;
using UnityEngine;

/// <summary>
/// Globally initializes all. LOADED AT VERY FIRST AND NEVER BEEN DESTROYED.
/// </summary>
namespace PataRoad.Core
{
    public class GlobalData : MonoBehaviour
    {
        public static UnityEngine.InputSystem.PlayerInput Input { get; private set; }
        public static PataponInfo PataponInfo { get; private set; } = new PataponInfo();
        [SerializeField]
        int _tipIndex = -1;
        public int TipIndex => _tipIndex;
        public static AudioSource GlobalAudioSource { get; private set; }
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GlobalAudioSource = GetComponentInChildren<AudioSource>();
            Input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            Items.ItemLoader.LoadAll();
        }
    }
}
