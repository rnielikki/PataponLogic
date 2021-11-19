using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
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
