using UnityEngine;

namespace PataRoad.Story
{
    class RightChoiceReward : MonoBehaviour
    {
        [SerializeField]
        Core.Items.ItemMetaData _data;
        [SerializeField]
        int _amount;

        public void Reward()
        {
            Core.Global.GlobalData.CurrentSlot.Inventory.AddMultiple(
                _data.ToItem(),
                _amount
                );
        }
    }
}
