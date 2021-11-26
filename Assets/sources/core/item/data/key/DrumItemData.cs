using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents Drum key item that is available. Without drum item, you can't use the drum.
    /// </summary>
    [CreateAssetMenu(fileName = "drum", menuName = "KeyItemData/Drum")]
    public class DrumItemData : KeyItemData
    {
        public override string Group => "Drum";
        [SerializeField]
        private Rhythm.DrumType _drum;
        public Rhythm.DrumType Drum => _drum;
    }
}
