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
/// [Colour.txt] contains 6-digit HEX (NO ALPHA) WITHOUT #
/// *---------------------------------------------------------------------------------
/// </remarks>
namespace PataRoad.Core.Map.Background
{
    public class BackgroundLoader : MonoBehaviour
    {
        const string _path = "Map/Backgrounds/";
        // Start is called before the first frame update
        public void Init(string theme)
        {
            var txt = Resources.Load<TextAsset>(_path + theme + "/colour");
            Camera.main.backgroundColor = ParseColor(txt.text);

            foreach (var bg in Resources.LoadAll<GameObject>(_path + theme))
            {
                SetTexture(Instantiate(bg, transform));
            }
        }

        private void SetTexture(GameObject obj)
        {
            var image = obj.GetComponent<UnityEngine.UI.Image>();
            if (image == null) return;

            var mainTex = image.material.mainTexture;
            var rect = obj.GetComponent<RectTransform>().rect;
            image.material.SetTextureScale("_MainTex", new Vector2(mainTex.height * rect.width / (mainTex.width * rect.height), 1));
        }
        private Color ParseColor(string colourHex)
        {
            try
            {
                if (colourHex.Length != 6)
                {
                    throw new System.ArgumentException("Length of the file is invalid.");
                }
                int[] colors = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    colors[i] = System.Convert.ToInt32(colourHex.Substring(i * 2, 2), 16);
                }
                return new Color((float)colors[0] / 255, (float)colors[1] / 255, (float)colors[2] / 255);
            }
            catch (System.Exception)
            {
                throw new System.ArgumentException("colour.txt in backgorund: Format is invalid. Make sure that it contains 6-digit HEX without any space!");
            }
        }
    }
}
