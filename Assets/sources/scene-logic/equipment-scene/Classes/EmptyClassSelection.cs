using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EmptyClassSelection : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        ClassSelectionUpdater _updater;
        public void OnSelect(BaseEventData eventData)
        {
            _updater.UpdateDescription(null);
        }
    }
}
