using System.Linq;

namespace PataRoad.Core.Global.Slots
{
    /// <summary>
    /// Represents lightweight metadata of <see cref="Slot"/>. These data also appears on saved slot list.
    /// </summary>
    /// <seealso cref="SlotMetaList"/>
    [System.Serializable]
    public class SlotMeta
    {
        [UnityEngine.SerializeField]
        private int _slotIndex;
        public int SlotIndex => _slotIndex;

        [UnityEngine.SerializeField]
        private long _playTime;
        public System.TimeSpan PlayTime => new System.TimeSpan(_playTime);

        [UnityEngine.SerializeField]
        private string _lastSavedTime;
        public string LastSavedTime => _lastSavedTime;

        [UnityEngine.SerializeField]
        private string _squadStatus;

        public string SquadStatus => _squadStatus;

        [UnityEngine.SerializeField]
        private string _lastMapName;
        public string LastMapName => _lastMapName;

        [UnityEngine.SerializeField]
        private string _almightyName;
        public string AlmightyName => _almightyName;

        public SlotMeta(Slot slot)
        {
            _playTime = slot.PlayTime;
            _squadStatus = string.Join(" / ", slot.PataponInfo.CurrentClasses.Select(cl => cl.ToString()));
            _lastMapName = (slot.MapInfo.LastMap?.MapData?.Name ?? _lastMapName) ?? "-";
            if (!slot.MapInfo.SuccededLast)
            {
                _lastMapName += " (F)";
            }
            _almightyName = slot.AlmightyName;
        }
        public void SetLastSavedTime()
        {
            _lastSavedTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
