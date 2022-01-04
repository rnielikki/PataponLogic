using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Main
{
    class MainMenuSelection : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        Button _button;
        public Button Button => _button;
        [SerializeField]
        RectTransform _selectionImage;
        RectTransform _selfTransform;
        [SerializeField]
        AudioClip _selectedSound;
        private void Awake()
        {
            _selfTransform = GetComponent<RectTransform>();
        }
        public void OnSelect(BaseEventData eventData)
        {
            var pos = _selectionImage.anchoredPosition;
            pos.y = _selfTransform.anchoredPosition.y;
            _selectionImage.anchoredPosition = pos;
            Core.Global.GlobalData.Sound.PlayInScene(_selectedSound);
        }
    }
}
