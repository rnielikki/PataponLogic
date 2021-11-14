using UnityEngine;

/// <summary>
/// Globally initializes all. LOADED AT VERY FIRST AND NEVER BEEN DESTROYED.
/// </summary>
namespace PataRoad.Core
{
    public class GlobalInitialization : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Items.ItemLoader.LoadAll();
            DontDestroyOnLoad(gameObject);
        }
    }
}
