using PataRoad.Core.Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    class WorldMapItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        Image _sprite;
        [SerializeField]
        Image _windSprite;
        [SerializeField]
        Text _text;
        [SerializeField]
        Button _button;
        WorldMapSelector _parent;
        private MapDataContainer _map;
        private Color _textColor;

        public void Init(MapDataContainer map, WorldMapSelector parent, Color textColor, Sprite weatherSprite)
        {
            _parent = parent;
            _map = map;
            _text.text = map.GetNameWithLevel();
            _textColor = textColor;
            _text.color = textColor;

            var clrs = _button.colors;
            clrs.selectedColor = textColor;
            _button.colors = clrs;

            _button.onClick.AddListener(StartMission);// -- start mission
            _sprite.sprite = weatherSprite;
            _windSprite.enabled = map.CurrentWind != Core.Map.Weather.WindType.None;
        }
        private void Start()
        {
            //GetComponentInChildren<WeatherUpdater>().UpdateWeather(_map.CurrentWeather, _map.CurrentWind != Core.Map.Weather.WindType.None);
        }
        public void Select() => _button.Select();
        private void StartMission()
        {
            Core.Global.GlobalData.MapInfo.Select(_map);
            UnityEngine.SceneManagement.SceneManager.LoadScene("EquipmentScreen");
        }
        public void HideIfNotRightType(MapType mapType)
        {
            gameObject.SetActive(_map.MapData.Type == mapType);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            _text.color = _textColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _parent.UpdateDescription(_map);
            _text.color = Color.black;
        }
    }
}
