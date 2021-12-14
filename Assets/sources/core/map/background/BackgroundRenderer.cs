using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Map.Background
{
    public class BackgroundRenderer : MonoBehaviour
    {
        Image _image;
        void Awake()
        {
            _image = GetComponent<Image>();
            //Still resize problem remains. If you can handle shader language, please replace to shader...
            var mainTex = _image.material.mainTexture;
            var rect = GetComponent<RectTransform>().rect;
            _image.material.SetTextureScale("_MainTex", new Vector2(mainTex.height * rect.width / (mainTex.width * rect.height), 1));
        }
    }
}
