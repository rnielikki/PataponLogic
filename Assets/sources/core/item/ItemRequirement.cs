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
        private int _defaultAmount;
        public virtual void Init()
        {
            _defaultAmount = Amount;
        }
        public virtual void SetRequirementByLevel(int level)
        {
            _amount = _defaultAmount + level - 1;
        }

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
