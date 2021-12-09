using PataRoad.Core.Global;
using PataRoad.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [Serializable]
    public class PataponInfo : IPlayerData
    {
        [SerializeField]
        private List<Class.ClassType> _currentClasses;
        public Class.ClassType[] CurrentClasses => _currentClasses.ToArray();
        public int ClassCount => _currentClasses.Count;

        /// <summary>
        /// How many groups (how many group of various classes) can go to fight.
        /// </summary>
        public const int MaxPataponGroup = 3;

        Dictionary<Class.ClassType, PataponClassInfo> _classInfoMap = new Dictionary<Class.ClassType, PataponClassInfo>();
        Dictionary<IItem, int> _amountMap = new Dictionary<IItem, int>();
        [SerializeReference]
        PataponClassInfo[] _classInfoForSerialization;

        public StringKeyItemData BossToSummon { get; set; }
        public StringKeyItemData CustomMusic { get; set; }

        [SerializeReference]
        int _summonIndex;
        [SerializeReference]
        int _musicIndex;

        public PataponInfo()
        {
            //Not serialized --
            foreach (Class.ClassType type in Enum.GetValues(typeof(Class.ClassType)))
            {
                _classInfoMap.Add(type, new PataponClassInfo(type));
            }

            _currentClasses = new List<Class.ClassType>()
            {
                Class.ClassType.Yaripon
            };
            Order();
        }
        public void ReplaceClass(Class.ClassType from, Class.ClassType to)
        {
            if (from == to) return;
            _currentClasses.Remove(from);
            _currentClasses.Add(to);
            Order();
        }
        public void AddClass(Class.ClassType type)
        {
            if (!_currentClasses.Contains(type) && _currentClasses.Count < MaxPataponGroup)
            {
                _currentClasses.Add(type);
            }
            Order();
        }
        public void RemoveClass(Class.ClassType type) => _currentClasses.Remove(type);
        private void Order()
        {
            _currentClasses = _currentClasses.OrderBy(c => (int)c).ToList();
        }

        internal bool ContainsClass(Class.ClassType type) => _currentClasses.Contains(type);

        public int GetEquippedCount(IItem item)
        {
            if (_amountMap.TryGetValue(item, out int res)) return res;
            else return 0;
        }
        public void UpdateClassEquipmentStatus(PataponData data, EquipmentData equipmentData)
        {
            data.Equip(equipmentData);
            GetClassInfo(data.Type).SetEquipmentInIndex(data.IndexInGroup, equipmentData);
        }
        public void UpdateGeneralEquipmentStatus(PataponData data, GeneralModeData generalModeData)
        {
            GetClassInfo(data.Type).SetGeneralModeData(generalModeData);
        }
        public IEnumerable<EquipmentData> GetCurrentEquipments(PataponData data)
        {
            return GetClassInfo(data.Type).GetEquipmentInIndex(data.IndexInGroup);
        }
        private PataponClassInfo GetClassInfo(Class.ClassType type)
        {
            if (!_classInfoMap.ContainsKey(type))
            {
                _classInfoMap.Add(type, new PataponClassInfo(type));
            }
            return _classInfoMap[type];
        }
        public string Serialize()
        {
            _classInfoForSerialization = _classInfoMap.Values.ToArray();
            _summonIndex = BossToSummon.Index;
            _musicIndex = CustomMusic.Index;
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            _classInfoMap.Clear();
            foreach (var classInfo in _classInfoForSerialization)
            {
                _classInfoMap.Add(classInfo.Class, classInfo);
            }
            BossToSummon = ItemLoader.GetItem<StringKeyItemData>(ItemType.Key, "Boss", _summonIndex);
            CustomMusic = ItemLoader.GetItem<StringKeyItemData>(ItemType.Key, "Music", _musicIndex);
        }
    }
}
