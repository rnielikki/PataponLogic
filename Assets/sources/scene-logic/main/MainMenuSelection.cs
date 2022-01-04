using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Main
{
    class MainMenuSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        Button _button;
        public Button Button => _button;
        [SerializeField]
        GameObject _selectionImage;
        [SerializeField]
        AudioClip _selectedSound;
        public void OnSelect(BaseEventData eventData)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_selectedSound);
            _selectionImage.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _selectionImage.SetActive(false);
        }
    }
}
