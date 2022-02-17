using UnityEngine;

namespace PataRoad.Story
{
    class NextStoryWithItemCondition : MonoBehaviour
    {
        [SerializeField]
        Core.Items.ItemMetaData _item;
        [SerializeField]
        StoryData _storyData;
        [SerializeField]
        bool _nextStoryIfHasItem;
        [SerializeField]
        bool _addItemIfNotExist;
        public void CheckAndKeepNextStory()
        {
            if ((Core.Global.GlobalData.CurrentSlot.Inventory.HasItem(_item.ToItem()) && !_nextStoryIfHasItem)
                || (!Core.Global.GlobalData.CurrentSlot.Inventory.HasItem(_item.ToItem()) && _nextStoryIfHasItem))
            {
                _storyData.SkipNextStory();
            }
            else if (_addItemIfNotExist)
            {
                Core.Global.GlobalData.CurrentSlot.Inventory.AddItem(_item.ToItem());
            }
        }
    }
}
