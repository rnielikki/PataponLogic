using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [Serializable]
    public class PataponInfo
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
        Dictionary<Items.IItem, int> _amountMap = new Dictionary<Items.IItem, int>();

        public PataponInfo()
        {
            if (PlayerPrefs.HasKey(SerializationKeys.PataponInfo))
            {
                //Serialize --
            }
            else
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

        public int GetEquippedCount(Items.IItem item)
        {
            if (_amountMap.TryGetValue(item, out int res)) return res;
            else return 0;
        }

        public void Serialize()
        {
            //_allClasses = _classInfoMap.Values.ToArray();
        }
        public void Deserialize()
        {
            /*
            _classInfoMap.Clear();
            foreach (var pataponClassInfo in _allClasses)
            {
                _classInfoMap.Add(pataponClassInfo.Class, pataponClassInfo);
            }
            */
        }
        /*
        public PataponClassEquipmentInfo GetEquipmentInfo(Class.ClassType type, int index)
        {
            return _classInfoMap[type].GetEquipmentInIndex(index);
        }
        */
    }
}
