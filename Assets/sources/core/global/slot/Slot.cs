using PataRoad.Core.Character.Patapons.Data;
using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Global.Slots
{
    /// <summary>
    /// Represents a whole save data. For summary, use <see cref="SlotMeta"/>.
    /// </summary>
    [System.Serializable]
    public class Slot : ISerializationCallbackReceiver
    {
        [SerializeField]
        private PataponInfo _pataponInfo;
        /// <summary>
        /// Patapon information, like headquarter and equipment status.
        /// </summary>
        public PataponInfo PataponInfo => _pataponInfo;

        [SerializeReference]
        RareponInfo _rareponInfo;
        /// <summary>
        /// Information of Rarepons. Separated from PataponInfo for avoiding serialization problem.
        /// </summary>
        public RareponInfo RareponInfo => _rareponInfo;

        [SerializeField]
        private Inventory _inventory;
        /// <summary>
        /// Inventory information. Saves current items in the game progress.
        /// </summary>
        public Inventory Inventory => _inventory;

        [SerializeField]
        private MapInfo _mapInfo;
        /// <summary>
        /// Saves currently open map and other data related to the map, like current weather.
        /// </summary>
        public MapInfo MapInfo => _mapInfo;

        [SerializeField]
        private long _playTime;
        /// <summary>
        /// Played time as seconds.
        /// </summary>
        public long PlayTime => _playTime;
        private float _startTime;

        [SerializeField]
        private string _almightyName;
        /// <summary>
        /// Name of the almighty, that defined in very first. This value is unique to the game data and shouldn't be modified after definition.
        /// </summary>
        public string AlmightyName => _almightyName;

        [SerializeReference]
        private GameProgress _progress;
        /// <summary>
        /// Saved progress of the game, which determines will be some speicific things open or not, like NPCs.
        /// </summary>
        public GameProgress Progress => _progress;

        /// <summary>
        /// Loads slot with initial game status.
        /// </summary>
        public static Slot CreateNewGame()
        {
            var slot = new Slot();
            //---------------------------------- Not serialized.
            slot._rareponInfo = RareponInfo.CreateNew();
            slot._pataponInfo = PataponInfo.CreateNew(slot._rareponInfo);
            slot._inventory = Inventory.CreateNew(); //must be loaded after item loader init
            slot._mapInfo = MapInfo.CreateNew();
            slot._playTime = 0;
            slot._startTime = (int)Time.realtimeSinceStartup;
            slot._almightyName = "";
            slot._progress = new GameProgress();

            return slot;
        }
        public static Slot LoadSlot(int index)
        {
            var slotIndex = index.ToString();
            if (!PlayerPrefs.HasKey(slotIndex))
            {
                throw new MissingReferenceException("No data exist in the slot index " + slotIndex);
            }
            var slot = JsonUtility.FromJson<Slot>(
                    PlayerPrefs.GetString(slotIndex)
                );
            SlotMetaList.SetLastSlotIndex(index);
            slot._startTime = (int)Time.realtimeSinceStartup;
            return slot;
        }
        public SlotMeta Save(int slotIndex)
        {
            _playTime += (long)(Time.realtimeSinceStartup - _startTime) * 10_000_000;
            _startTime = Time.realtimeSinceStartup;
            PlayerPrefs.SetString(slotIndex.ToString(),
                    JsonUtility.ToJson(this)
                );

            //And update meta
            var slotMeta = GetSlotMeta();
            SlotMetaList.Save(slotMeta, slotIndex);
            PlayerPrefs.Save();
            return slotMeta;
        }
        public void SetAlmightyName(string name)
        {
            if (string.IsNullOrWhiteSpace(_almightyName)) _almightyName = name;
        }
        public SlotMeta GetSlotMeta() => new SlotMeta(this);
        private void SerializeAll()
        {
            _rareponInfo.Serialize();
            _pataponInfo.Serialize();
            _inventory.Serialize();
            _mapInfo.Serialize();
        }
        private void DeserializeAll()
        {
            _rareponInfo.Deserialize();
            _pataponInfo.Deserialize();
            _pataponInfo.SetRareponInfo(_rareponInfo);
            _inventory.Deserialize();
            _mapInfo.Deserialize();
        }

        public void OnBeforeSerialize()
        {
            SerializeAll();
        }

        public void OnAfterDeserialize()
        {
            DeserializeAll();
        }
    }
}
