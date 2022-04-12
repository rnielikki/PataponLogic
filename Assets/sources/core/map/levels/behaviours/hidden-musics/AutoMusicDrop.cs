using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Map.Levels.MusicDrops
{
    public class AutoMusicDrop : MonoBehaviour
    {
        [SerializeField]
        ItemDropData _template;
        [SerializeField]
        int _musicIndex;
        IItem _item;
        // Use this for initialization
        void Awake()
        {
            _item = ItemLoader.GetItem(ItemType.Key, "Music", _musicIndex);
        }
        public void DropMusic() => DropMusic(transform.position);
        protected void DropMusic(Vector3 position)
        {
            if (!Global.GlobalData.CurrentSlot.Inventory.HasItem(_item))
            {
                ItemDrop.DropItem(_template, position, true, _item);
            }
        }
    }
}