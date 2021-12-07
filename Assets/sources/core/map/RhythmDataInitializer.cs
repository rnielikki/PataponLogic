using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class RhythmDataInitializer : MonoBehaviour
    {
        void Awake()
        {
            var drums = GlobalData.Inventory.GetItemsByType(Items.ItemType.Key, "Drum");
            var allDrums = drums.Select(drm => (drm.item as Items.DrumItemData).Drum);
            foreach (var input in GetComponentsInChildren<Rhythm.RhythmInput>())
            {
                if (!allDrums.Contains(input.DrumType)) input.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
