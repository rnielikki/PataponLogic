using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [System.Serializable]
    public class PataponInfo
    {
        [SerializeField]
        private List<Class.ClassType> _currentClasses;
        public Class.ClassType[] CurrentClasses => _currentClasses.ToArray();
        /// <summary>
        /// How many groups (how many group of various classes) can go to fight.
        /// </summary>
        public const int MaxPataponGroup = 3;

        //[SerializeReference]
        //PataponCurrentClassInfo[] _allClasses;

        //Dictionary<Class.ClassType, PataponCurrentClassInfo> _classInfoMap = new Dictionary<Class.ClassType, PataponCurrentClassInfo>();

        public PataponInfo()
        {
            //Not serialized --
            /*
            foreach (Class.ClassType type in System.Enum.GetValues(typeof(Class.ClassType)))
            {
                if (type != Class.ClassType.Any) _classInfoMap.Add(type, new PataponCurrentClassInfo(type));
            }
            */
            _currentClasses = new List<Class.ClassType>()
            {
                Class.ClassType.Mahopon,
                Class.ClassType.Robopon
            };
            //Serialize --
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
