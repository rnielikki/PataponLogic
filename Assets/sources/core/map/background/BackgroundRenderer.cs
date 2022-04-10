using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Map.Background
{
    public class BackgroundRenderer : MonoBehaviour
    {
        [SerializeField]
        Image _image;
        [SerializeField]
        RectTransform _rectTransform;
        internal void SetMaterial(Material material)
        {
            LoadSizeInfo(material);

            _image.material = material;
            var mainTex = _image.material.mainTexture;
            var rect = _rectTransform.rect;

            _image.material.SetTextureScale("_MainTex", new Vector2(mainTex.height * rect.width / (mainTex.width * rect.height), 1));

        }
        private void LoadSizeInfo(Material material)
        {
            bool useFullHeight = material.GetInteger("_FullHeight") != 0;
            int bgHeight = material.GetInteger("_BgHeight");
            if (useFullHeight)
            {
                _rectTransform.anchorMax = Vector2.one;
                _rectTransform.offsetMax = new Vector2(0, -bgHeight);
            }
            else
            {
                _rectTransform.anchorMax = Vector2.right;
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bgHeight);
            }
        }
    }
}
