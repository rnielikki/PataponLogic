using UnityEngine;

namespace PataRoad.Core.Map.Background
{
    /// <summary>
    /// Doesn't contain background materials. MUST BE IN THE THEME DIRECTORY WITH MATERIALS.
    /// also set FILE NAME as "data"
    /// </summary>
    [CreateAssetMenu(fileName = "bgData", menuName = "Background")]
    public class BackgroundData : ScriptableObject
    {
        [SerializeField]
        Color _colorGround;
        public Color ColorGround => _colorGround;
        [SerializeField]
        Color _colorTop;
        public Color ColorTop => _colorTop;
        [SerializeField]
        bool _useMud;
        public bool UseMud => _useMud;
        [SerializeField]
        Color _mudColor;
        public Color MudColor => _mudColor;
    }
}