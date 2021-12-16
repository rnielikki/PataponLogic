using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    internal class WorldMapMenu : MonoBehaviour
    {
        [SerializeField]
        private bool _showAll;
        [SerializeField]
        private Core.Map.MapType _filter;
        [SerializeField]
        Image _image;
        [SerializeField]
        Sprite _imageOnSelected;
        Sprite _imageOnNotSelected;

        private void Start()
        {
            _imageOnNotSelected = _image.sprite;
        }
        public Core.Map.MapType? MarkAsCurrentAndGetFilter()
        {
            _image.sprite = _imageOnSelected;
            if (_showAll) return null;
            else return _filter;
        }
        public void MarkAsNonCurrent()
        {
            _image.sprite = _imageOnNotSelected;
        }
    }
}
