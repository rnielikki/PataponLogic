using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents key item, which contains data as string. Used for boss summon data and music data.
    /// </summary>
    [CreateAssetMenu(fileName = "stringData", menuName = "Item/KeyItemData/StringData")]
    public class StringKeyItemData : KeyItemData
    {
        [SerializeField]
        private string _data;
        public string Data => _data;
    }
}
