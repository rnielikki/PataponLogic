using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.Common.Navigator
{
    /// <summary>
    /// A very simple class for playing sound on select.
    /// </summary>
    public class SelectSound : MonoBehaviour, ISelectHandler
    {
        [SerializeField]
        AudioClip _selectSound;
        public void OnSelect(BaseEventData eventData)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
        }
    }
}
