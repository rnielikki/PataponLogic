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

        [SerializeReference]
        RareponInfo _rareponInfo;
        public RareponInfo RareponInfo => _rareponInfo;

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
            _rareponInfo = new RareponInfo();
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
            if (item != null && _amountMap.TryGetValue(item, out var res)) return res;
            else return 0;
        }
        public void UpdateClassEquipmentStatus(PataponData data, EquipmentData equipmentData)
        {
            var type = equipmentData.Type;
            var oldEquipment = data.EquipmentManager.GetEquipmentData(type);
            if (oldEquipment == equipmentData) return;

            if (type == Equipments.EquipmentType.Rarepon && (oldEquipment == null || oldEquipment.Index == 0))
            {
                var helm = data.EquipmentManager.GetEquipmentData(Equipments.EquipmentType.Helm);
                RemoveFromAmountMapData(helm);
            }
            else
            {
                RemoveFromAmountMapData(oldEquipment);
            }

            data.Equip(equipmentData);
            AddToAmountMapData(equipmentData);

            GetClassInfo(data.Type).SetEquipmentInIndex(data.IndexInGroup, equipmentData);
        }
        public (Class.ClassType type, int index) GetExchangablePataponMetaData(IEnumerable<Class.ClassType> allAvailableClasses,
            PataponData currentPataponData, EquipmentData data)
        {
            //1. not in squad
            var resultnPataponMeta = GetPataponMetaFrom(allAvailableClasses.Except(_currentClasses), data);
            if (resultnPataponMeta != null) return resultnPataponMeta.Value;

            var currentClassWrap = new Class.ClassType[] { currentPataponData.Type };

            //2. in squad but not current 
            resultnPataponMeta = GetPataponMetaFrom(_currentClasses.Except(currentClassWrap), data);
            if (resultnPataponMeta != null) return resultnPataponMeta.Value;

            //3. Finally from self. final result shouldn't be null
            resultnPataponMeta = GetPataponMetaFrom(currentClassWrap, data, currentPataponData);
            return resultnPataponMeta.Value;
        }
        public bool IsEquippedOutside(IEnumerable<Class.ClassType> allAvailableClasses, EquipmentData data, out (Class.ClassType type, int index) result)
        {
            var res = GetPataponMetaFrom(allAvailableClasses.Except(_currentClasses), data);
            if (res != null)
            {
                result = res.Value;
            }
            else
            {
                result = (Class.ClassType.Yaripon, -1);
            }
            return res != null;
        }
        public bool HasBestEquipmentInside(Class.ClassType classType, Equipments.EquipmentType equipmentType, int LevelGroup)
        {
            return _classInfoMap[classType].GetMaxEquipmentLevel(equipmentType) >= LevelGroup;
        }

        private (Class.ClassType type, int index)? GetPataponMetaFrom(IEnumerable<Class.ClassType> classes, EquipmentData equipmentData, PataponData pataponData = null)
        {
            foreach (var classType in classes)
            {
                var allHolders = _classInfoMap[classType].GetAllHolders(equipmentData);
                if (allHolders.Count > 0)
                {
                    int finalIndex;
                    if (pataponData != null) finalIndex = allHolders.Last(holder => holder != pataponData.IndexInGroup);
                    else finalIndex = allHolders[allHolders.Count - 1];
                    return (classType, finalIndex);
                }
            }
            return null;
        }
        public void ExchangeClassEquipmentStatus(PataponData oldHolder, PataponData newHolder, EquipmentData equipmentData)
        {
            var oldEquipment = newHolder.EquipmentManager.GetEquipmentData(equipmentData.Type);

            oldHolder.Equip(oldEquipment);
            GetClassInfo(oldHolder.Type).SetEquipmentInIndex(oldHolder.IndexInGroup, oldEquipment);

            newHolder.Equip(equipmentData);
            GetClassInfo(newHolder.Type).SetEquipmentInIndex(newHolder.IndexInGroup, equipmentData);
        }
        private void RemoveFromAmountMapData(EquipmentData equipmentData)
        {
            if (equipmentData == null || equipmentData.Index == 0) return;
            //well amountMap must contain the equipment and data.
            _amountMap[equipmentData]--;
            if (_amountMap[equipmentData] == 0) _amountMap.Remove(equipmentData);
        }
        private void AddToAmountMapData(EquipmentData equipmentData)
        {
            if (equipmentData == null || equipmentData.Index == 0) return;
            if (_amountMap.ContainsKey(equipmentData))
            {
                _amountMap[equipmentData]++;
            }
            else
            {
                _amountMap.Add(equipmentData, 1);
            }
        }
        public GeneralModeData GetGeneralMode(Class.ClassType type) => GetClassInfo(type).GeneralModeData;
        public bool HasGeneralMode(GeneralModeData generalModeData, out Class.ClassType classType)
        {
            var duplication = _classInfoMap.Values.Where(cl => cl.GeneralModeData == generalModeData);
            if (duplication.Any())
            {
                classType = duplication.Single().Class;
                return true;
            }
            else
            {
                classType = Class.ClassType.Yaripon;
                return false;
            }
        }
        public void UpdateGeneralEquipmentStatus(Class.ClassType type, GeneralModeData generalModeData)
        {
            if (generalModeData != null && HasGeneralMode(generalModeData, out Class.ClassType havingClassType))
            {
                var oldClassInfo = GetClassInfo(type);
                var newClassInfo = GetClassInfo(havingClassType);
                //SWAP
                var temp = oldClassInfo.GeneralModeData;
                oldClassInfo.SetGeneralModeData(generalModeData);
                newClassInfo.SetGeneralModeData(temp);
                return;
            }
            GetClassInfo(type).SetGeneralModeData(generalModeData);
        }
        public IEnumerable<EquipmentData> GetCurrentEquipments(PataponData data)
        {
            return GetClassInfo(data.Type).GetEquipmentInIndex(data.IndexInGroup);
        }
        public int GetAttackTypeIndex(Class.ClassType classType) =>
            GetClassInfo(classType).AttackTypeIndex;

        public void SetAttackTypeIndex(Class.ClassType classType, int index) =>
            GetClassInfo(classType).AttackTypeIndex = index;

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
            _summonIndex = BossToSummon?.Index ?? -1;
            _musicIndex = CustomMusic?.Index ?? -1;
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            _classInfoMap.Clear();
            foreach (var classInfo in _classInfoForSerialization)
            {
                _classInfoMap.Add(classInfo.Class, classInfo);
            }
            if (_summonIndex != -1) BossToSummon = ItemLoader.GetItem<StringKeyItemData>(ItemType.Key, "Boss", _summonIndex);
            if (_musicIndex != -1) CustomMusic = ItemLoader.GetItem<StringKeyItemData>(ItemType.Key, "Music", _musicIndex);

            foreach (var equipmentData in _classInfoForSerialization.SelectMany(item => item.GetAllEquipments()))
            {
                AddToAmountMapData(equipmentData);
            }
        }
    }
}
