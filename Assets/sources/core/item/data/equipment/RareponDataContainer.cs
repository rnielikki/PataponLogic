namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// for serialization. It contains LEVEL.
    /// </summary>
    [System.Serializable]
    public class RareponDataContainer
    {
        [UnityEngine.SerializeField]
        private int _rareponIndex;
        public int RareponIndex => _rareponIndex;
        public RareponData Data { get; private set; }
        [UnityEngine.SerializeField]
        private int _level;
        public int Level => _level;
        /// <summary>
        /// Not opened rarepon index.
        /// </summary>
        /// <param name="index"></param>
        public RareponDataContainer(int index)
        {
            _rareponIndex = index;
            _level = 1;
        }
        /// <summary>
        /// Must call this after serialization.
        /// </summary>
        internal void LoadData()
        {
            Data = Patapons.Data.RareponInfo.GetRareponData(_rareponIndex, _level);
        }
        /// <summary>
        /// Level Up and returns the Rarepon data. If level is greather than 3, it doesn't level up and returns current data.
        /// </summary>
        /// <returns>Level</returns>
        internal RareponData LevelUp()
        {
            if (!CanLevelUp()) return Data;
            var level = _level + 1;
            var data = Patapons.Data.RareponInfo.GetRareponData(_rareponIndex, _level);
            if (data != null)
            {
                Data = data;
                _level = level;
            }
            Global.GlobalData.CurrentSlot.PataponInfo.RefreshRarepons(this);
            return Data;
        }
        public bool CanLevelUp() => Level < 3;
    }
}
