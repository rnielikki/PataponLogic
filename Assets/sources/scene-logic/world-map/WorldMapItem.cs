using PataRoad.Common.GameDisplay;
using PataRoad.Core.Map;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    class WorldMapItem : MonoBehaviour, IDeselectHandler, IScrollListElement
    {
        [SerializeField]
        Image _sprite;
        [SerializeField]
        Image _windSprite;
        [SerializeField]
        Text _text;
        [SerializeField]
        Button _button;
        [SerializeField]
        AudioClip _selectSound;
        [SerializeField]
        AudioClip _enterSound;

        WorldMapSelector _parent;
        RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;
        ScrollList _scrollList;
        private MapDataContainer _map;
        private Color _textColor;
        public int Index { get; set; }
        public MapDataContainer Map => _map;
        public Selectable Selectable => _button;

        public void Init(MapDataContainer map, WorldMapSelector parent, ScrollList scrollList, Color textColor, Sprite weatherSprite)
        {
            _rectTransform = GetComponent<RectTransform>();
            _scrollList = scrollList;
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

            _button.onClick.AddListener(() => Core.Global.GlobalData.Sound.PlayGlobal(_enterSound));
        }
        public void Select() => _button.Select();
        private void StartMission()
        {
            Core.Global.GlobalData.MapInfo.Select(_map);
            UnityEngine.SceneManagement.SceneManager.LoadScene("EquipmentScreen");
        }
        public bool HideIfNotType(MapType mapType)
        {
            bool isType = _map.MapData.Type == mapType;
            gameObject.SetActive(isType);
            return isType;
        }
        public void OnDeselect(BaseEventData eventData)
        {
            _text.color = _textColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            _parent.UpdateDescription(_map);
            _text.color = Color.black;
            _scrollList.Scroll(this);
        }
        private void OnDisable()
        {
            _text.color = _textColor;
        }
    }
}
