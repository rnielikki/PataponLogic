using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level1 : MonoBehaviour
    {
        [SerializeField]
        Character.Structure _structure;

        private void Start()
        {
            //No ponpon song yet
            if (!Global.GlobalData.CurrentSlot.Inventory.HasItem(
                Items.ItemLoader.GetItem(Items.ItemType.Key, "Song", 1)
                ))
            {
                var dropBehaviour = GetComponent<Items.ItemDropBehaviour>();
                (dropBehaviour.DropData[0] as Items.SongItemDropData).OnSongComplete
                    .AddListener(() => _structure.Die());

                dropBehaviour.Drop();
            }
            else
            {
                Destroy(_structure.gameObject);
            }
        }
    }
}
