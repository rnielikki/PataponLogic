using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Using for item requirement, for Unity inspector.
    /// </summary>
    /// <note>Make the array public to use this.</note>
    [System.Serializable]
    public abstract class ItemRequirement
    {
        public abstract IItem Item { get; }
        [SerializeField]
        int _amount;
        public int Amount => _amount;
    }
}
