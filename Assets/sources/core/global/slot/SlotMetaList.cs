﻿namespace PataRoad.Core.Global.Slots
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
            else result = UnityEngine.JsonUtility.FromJson<SlotMetaList>(data);
            _current = result;
            return result;
        }
        public static void Save(SlotMeta meta)
        {
            meta.SetLastSavedTime();
            _current._slotMeta[meta.SlotIndex] = meta;
        }
    }
}
