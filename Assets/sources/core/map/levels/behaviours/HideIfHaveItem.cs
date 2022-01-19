using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class HideIfHaveItem : MonoBehaviour
    {
        [SerializeField]
        Items.ItemMetaData _data;
        private void Start()
        {
            gameObject.SetActive(!Global.GlobalData.CurrentSlot.Inventory.HasItem(_data.ToItem()));
        }
    }
}
