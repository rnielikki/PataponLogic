using PataRoad.Core.Map;
using PataRoad.Core.Map.Weather;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.SceneLogic.WorldMap
{
    class WorldMapSelector : MonoBehaviour
    {
        [SerializeField]
        GameObject _listElement;
        [SerializeField]
        MapDescription _description;
        [SerializeField]
        Common.GameDisplay.ScrollList _scrollList;
        [Header("Colors")]
        [SerializeField]
        Color _huntColor;
        [SerializeField]
        Color _bossColor;
        [SerializeField]
        Color _battleColor;

        [Header("Weather sprites")]
        [SerializeField]
        Sprite _clearWeatherImage;
        [SerializeField]
        Sprite _rainWeatherImage;
        [SerializeField]
        Sprite _stormWeatherImage;
        [SerializeField]
        Sprite _snowWeatherImage;
        [SerializeField]
        Sprite _fogWeatherImage;

        private List<WorldMapItem> _items;
        internal void UpdateDescription(MapDataContainer map) => _description.UpdateDescription(map);
        private void Start()
        {
            _items = new List<WorldMapItem>();
            var nextMission = Core.Global.GlobalData.CurrentSlot.MapInfo.NextMap;
            bool _selected = false;
            int index = -1;
            WorldMapItem selectedItem = null;
            foreach (var map in Core.Global.GlobalData.CurrentSlot.MapInfo.GetAllAvailableMaps())
            {
                var mapItem = Instantiate(_listElement, transform).GetComponent<WorldMapItem>();
                mapItem.Init(
                    map,
                    this,
                    _scrollList,
                    GetColorForMap(map.MapData.Type),
                    GetSpriteImageForWeather(map.Weather.CurrentWeather));
                _items.Add(mapItem);
                if (!_selected && map == nextMission)
                {
                    selectedItem = mapItem;
                    _selected = true;
                }
                mapItem.Index = ++index;
            }
            var lastItem = selectedItem;
            if (selectedItem == null) lastItem = _items[_items.Count - 1];
            _scrollList.Init(lastItem);
            _scrollList.SetMaximumScrollLength(index, lastItem);
            lastItem.Select();
        }
        public void Filter(MapType mapType)
        {
            WorldMapItem _last = null;
            int index = -1;
            foreach (var item in _items)
            {
                if (item.HideIfNotType(mapType))
                {
                    item.Index = ++index;
                    _last = item;
                }
            }
            _scrollList.SetMaximumScrollLength(index, _last);
        }
        public void ShowNotCleared()
        {
            WorldMapItem _last = null;
            int index = -1;
            foreach (var item in _items)
            {
                if (item.HideIfCleared())
                {
                    item.Index = ++index;
                    _last = item;
                }
            }
            _scrollList.SetMaximumScrollLength(index, _last);
        }
        public void ShowAll()
        {
            int index = -1;
            foreach (var item in _items)
            {
                item.Index = ++index;
                item.gameObject.SetActive(true);
            }

            var nextMission = Core.Global.GlobalData.CurrentSlot.MapInfo.NextMap;
            var itemIndex = _items.FindLastIndex(item => item.Map == nextMission);
            if (itemIndex < 0) itemIndex = _items.Count - 1;
            _scrollList.SetMaximumScrollLength(index, _items[itemIndex]);
        }
        private Color GetColorForMap(MapType mapType) =>
            mapType switch
            {
                MapType.Battle => _battleColor,
                MapType.Boss => _bossColor,
                MapType.Hunt => _huntColor,
                _ => throw new System.NotImplementedException()
            };
        private Sprite GetSpriteImageForWeather(WeatherType weatherType) =>
            weatherType switch
            {
                WeatherType.Clear => _clearWeatherImage,
                WeatherType.Rain => _rainWeatherImage,
                WeatherType.Storm => _stormWeatherImage,
                WeatherType.Snow => _snowWeatherImage,
                WeatherType.Fog => _fogWeatherImage,
                _ => throw new System.NotImplementedException()
            };
    }
}
