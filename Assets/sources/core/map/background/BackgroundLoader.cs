using UnityEngine;

/// <summary>
/// Loads background by theme name.
/// </summary>
/// <remarks>
/// The theme must exist in the <see cref="_path"/> (theme path).
/// This theme contains some game objects that used for background, as well as "colour.txt" file, which reperesents background colour.
/// Each game object contains image, texture on *rect transform*. (See 'Ruins' directory for example)
/// Works even if theme game object is zero, but *colour.txt* SHOULD EXIST AT LEAST!
/// * --------------------------------------------------------------------------------
/// save <see cref="BackgroundData"/>as [data] on the theme directory.
/// *---------------------------------------------------------------------------------
/// </remarks>
namespace PataRoad.Core.Map.Background
{
    public class BackgroundLoader : MonoBehaviour
    {
        public const string DesertName = "Desert";
        const string _path = "Map/Backgrounds/";
        public static string CurrentTheme { get; private set; }
        private const int BgAmount = 4;
        [SerializeField]
        UnityEngine.UI.Image _topSkyImage;
        [SerializeField]
        UnityEngine.UI.Image _mudImage;
        public void Init(string theme)
        {
            CurrentTheme = theme;
            var bgData = Resources.Load<BackgroundData>(_path + theme + "/data");
            //color set
            Camera.main.backgroundColor = bgData.ColorGround;
            _topSkyImage.color = bgData.ColorTop;

            //ground set
            _mudImage.gameObject.SetActive(bgData.UseMud);
            if (bgData.UseMud) _mudImage.color = bgData.MudColor;

            //image set
            BackgroundRenderer[] children = GetComponentsInChildren<BackgroundRenderer>();
            Material[] materials = Resources.LoadAll<Material>(_path + theme);

            if (children.Length != materials.Length || materials.Length != BgAmount)
            {
                throw new MissingComponentException(
                    $"The material ({materials.Length}) and bg ({children.Length}) amount must be {BgAmount}");
            }

            for (int i = 0; i < BgAmount; i++)
            {
                children[i].SetMaterial(materials[i]);
            }
        }
        private void OnDestroy()
        {
            CurrentTheme = null;
        }
    }
}
