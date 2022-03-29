using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.ItemExchange
{
    public class ItemExchangeToggleMenu : MonoBehaviour
    {
        [SerializeField]
        bool _isGem;
        public bool IsGem => _isGem;
        [SerializeField]
        FromMaterialType _fromMaterial;
        public FromMaterialType Material => _fromMaterial;

        [SerializeField]
        Image _image;
        [SerializeField]
        Button _button;
        [SerializeField]
        ExchangeMaterialLoader _loader;
        [SerializeField]
        Sprite _defaultSprite;
        [SerializeField]
        Sprite _toggeldSprite;
        [SerializeField]
        ItemExchangeWindow _window;
        internal ItemExchangeWindow Window => _window;
        public void UnToggle()
        {
            _image.sprite = _defaultSprite;
        }
        public void ToggleToThis()
        {
            _window.UnToggleOthers();
            _image.sprite = _toggeldSprite;
            _loader.LoadMaterials(this);
        }
    }
}