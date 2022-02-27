using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Using for item requirement, for Unity inspector.
    /// </summary>
    /// <note>Make the array public to use this.</note>
    [System.Serializable]
    public abstract class ItemRequirement : ISerializationCallbackReceiver
    {
        public abstract IItem Item { get; }
        [SerializeField]
        protected int _amount;
        public int Amount => _amount;
        public abstract void Init();
        public abstract void SetRequirementByLevel(int level);

        public void OnBeforeSerialize()
        {
            //nothing,
        }

        public void OnAfterDeserialize()
        {
            Init();
        }
    }
}
