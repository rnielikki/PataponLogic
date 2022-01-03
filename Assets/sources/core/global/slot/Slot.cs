using PataRoad.Core.Character.Patapons.Data;
using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Global.Slots
{
    /// <summary>
    /// Represents a whole save data. For summary, use <see cref="SlotMeta"/>.
    /// </summary>
    [System.Serializable]
    public class Slot
    {
        [SerializeField]
        private PataponInfo _pataponInfo;
        /// <summary>
        /// Patapon information, like headquarter and equipment status.
        /// </summary>
        public PataponInfo PataponInfo => _pataponInfo;

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
        private System.TimeSpan _playTime;
        /// <summary>
        /// Played time as seconds.
        /// </summary>
        public System.TimeSpan PlayTime => _playTime;
        private int _startTime;

        [SerializeField]
        private string _almightyName;
        public string AlmightyName => _almightyName;

        /// <summary>
        /// Loads slot with initial game status.
        /// </summary>
        public static Slot CreateNewGame()
        {
            var slot = new Slot();
            //---------------------------------- Not serialized.
            slot._pataponInfo = PataponInfo.CreateNew();
            slot._inventory = Inventory.CreateNew(); //must be loaded after item loader init
            slot._mapInfo = MapInfo.CreateNew();
            slot._playTime = new System.TimeSpan(0);
            slot._almightyName = "Kamipon";

            return slot;
        }
        public static Slot LoadSlot(int index)
        {
            var slotIndex = index.ToString();
            if (!PlayerPrefs.HasKey(slotIndex))
            {
                return CreateNewGame();
                //throw new MissingReferenceException("No data exist in the slot index " + slotIndex);
            }
            var slot = JsonUtility.FromJson<Slot>(
                    PlayerPrefs.GetString(slotIndex)
                );
            slot._startTime = (int)Time.realtimeSinceStartup;
            slot.DeserializeAll();
            return slot;
        }
        public void Save(int slotIndex)
        {
            _playTime += new System.TimeSpan((int)Time.realtimeSinceStartup - _startTime);
            SerializeAll();
            PlayerPrefs.SetString(slotIndex.ToString(),
                    JsonUtility.ToJson(this)
                );

            //And update meta
            SlotMetaList.Save(GetSlotMeta());
            PlayerPrefs.Save();
        }
        public SlotMeta GetSlotMeta() => new SlotMeta(this);
        private void SerializeAll()
        {
            _pataponInfo.Serialize();
            _inventory.Serialize();
            _mapInfo.Serialize();
        }
        private void DeserializeAll()
        {
            _pataponInfo.Deserialize();
            _inventory.Deserialize();
            _mapInfo.Deserialize();
        }
    }
}
