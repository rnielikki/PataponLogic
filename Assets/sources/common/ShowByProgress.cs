using UnityEngine;

namespace PataRoad.Common
{
    /// <summary>
    /// Hides if the progress isn't fulfilled.
    /// </summary> 
    class ShowByProgress : MonoBehaviour
    {
        [SerializeField]
        Core.Global.Slots.GameProgressType _progressType;
        private void Start()
        {
            gameObject.SetActive(Core.Global.GlobalData.CurrentSlot.Progress.IsOpen(_progressType));
        }
    }
}
