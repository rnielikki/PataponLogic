namespace PataRoad.Core.Global.Slots
{
    /// <summary>
    /// Represents list of <see cref="SlotMeta"/>. Main menu directly read from this.
    /// </summary>
    [System.Serializable]
    public class SlotMetaList
    {
        [UnityEngine.SerializeReference]
        private SlotMeta[] _slotMeta;
        public static SlotMeta[] SlotMeta => _current._slotMeta;
        public const int SlotSize = 8;
        private static SlotMetaList _current { get; set; }
        public static bool HasSave { get; private set; }
        [UnityEngine.SerializeField]
        private int _lastSlotIndex;
        public static int LastSlotIndex => _current._lastSlotIndex;
        private SlotMetaList()
        {
            _slotMeta = new SlotMeta[SlotSize];
        }
        public static SlotMetaList Load()
        {
            var data = UnityEngine.PlayerPrefs.GetString("Saves");
            SlotMetaList result;
            if (string.IsNullOrEmpty(data))
            {
                result = new SlotMetaList();
            }
            else
            {
                result = UnityEngine.JsonUtility.FromJson<SlotMetaList>(data);
                HasSave = true;
            }
            _current = result;
            return result;
        }
        public static bool HasDataInIndex(int index) => index >= 0 && index < SlotSize && _current._slotMeta[index] != null;
        internal static void SetLastSlotIndex(int index) => _current._lastSlotIndex = index;
        internal static void Save(SlotMeta meta, int index)
        {
            if (index < 0 || index >= SlotSize) index = meta.SlotIndex;
            meta.SetLastSavedTime();
            _current._slotMeta[index] = meta;
            UnityEngine.PlayerPrefs.SetString("Saves",
                UnityEngine.JsonUtility.ToJson(_current)
            );
            _current._lastSlotIndex = index;
            HasSave = true;
        }
    }
}
