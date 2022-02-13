using UnityEngine;

namespace PataRoad.AppDebug
{
    class ItemAdder : MonoBehaviour
    {
        [SerializeField]
        Core.Items.ItemMetaData[] _metas;
        public void AddItem()
        {
            foreach (var meta in _metas)
            {
                var realItem = meta.ToItem();
                if (realItem == null)
                {
                    Debug.Log($"{meta} is null");
                }
                else
                {
                    Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(realItem);
                    Debug.Log(realItem.Name + " added");
                }
            }
        }

    }
}
