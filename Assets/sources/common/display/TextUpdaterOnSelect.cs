using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class TextUpdaterOnSelect : MonoBehaviour, UnityEngine.EventSystems.ISelectHandler
    {
        [UnityEngine.SerializeField]
        TMPro.TextMeshProUGUI _labelField;
        [UnityEngine.SerializeField]
        string _textToUpdate;
        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            _labelField.text = _textToUpdate;
        }
    }
}
