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
            var nextMission = Core.Global.GlobalData.MapInfo.NextMap;
            bool _selected = false;
            foreach (var map in Core.Global.GlobalData.MapInfo.GetAllAvailableMaps())
            {
                var mapItem = Instantiate(_listElement, transform).GetComponent<WorldMapItem>();
                mapItem.Init(map, this, GetColorForMap(map.MapData.Type), GetSpriteImageForWeather(map.CurrentWeather));
                _items.Add(mapItem);
                if (map == nextMission)
                {
                    mapItem.Select();
                    _selected = true;
                }
            }
            if (!_selected) _items[_items.Count - 1].Select();
        }
        public void Filter(MapType mapType)
        {
            WorldMapItem _last = null;
            foreach (var item in _items)
            {
                if (item.HideIfNotType(mapType))
                {
                    _last = item;
                }
            }
            if (_last != null)
            {
                _last.Select();
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }
        public void ShowAll()
        {
            foreach (var item in _items)
            {
                item.gameObject.SetActive(true);
            }
            _items[_items.Count - 1].Select();
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
