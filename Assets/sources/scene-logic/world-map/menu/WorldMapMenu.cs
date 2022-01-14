using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    internal class WorldMapMenu : MonoBehaviour
    {
        [SerializeField]
        private WorldMapFilterType _filter;
        [SerializeField]
        Image _image;
        [SerializeField]
        Sprite _imageOnSelected;
        Sprite _imageOnNotSelected;

        private void Start()
        {
            _imageOnNotSelected = _image.sprite;
        }
        public WorldMapFilterType MarkAsCurrentAndGetFilter()
        {
            _image.sprite = _imageOnSelected;
            return _filter;
        }
        public void MarkAsNonCurrent()
        {
            _image.sprite = _imageOnNotSelected;
        }
    }
}
