using UnityEngine;

/// <summary>
/// Globally initializes all. LOADED AT VERY FIRST AND NEVER BEEN DESTROYED.
/// </summary>
namespace PataRoad.Core
{
    public class GlobalData : MonoBehaviour
    {
        public static UnityEngine.InputSystem.PlayerInput Input { get; private set; }
        [SerializeField]
        int _tipIndex = -1;
        public int TipIndex => _tipIndex;
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Input = GetComponent<UnityEngine.InputSystem.PlayerInput>();
            Items.ItemLoader.LoadAll();
        }
    }
}
