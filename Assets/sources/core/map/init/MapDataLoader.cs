using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class MapDataLoader : MonoBehaviour
    {
        [SerializeField]
        int _musicThemeIndex;
        private void Awake()
        {
            FindObjectOfType<Rhythm.Bgm.RhythmBgmPlayer>().MusicTheme = LoadItem<StringKeyItemData>("Music", _musicThemeIndex).Data;
        }
        private T LoadItem<T>(string group, int index) where T : IItem
        {
            var item = ItemLoader.GetItem<T>(ItemType.Key, group, index);
            if (item == null)
            {
                throw new MissingComponentException($"The item with group {group}/{index} doesn't exist!");
            }
            return item;
        }
    }
}
