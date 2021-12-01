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
        public IEnumerable<Class.ClassType> CurrentClasses => _currentClasses;

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
                Class.ClassType.Dekapon,
                Class.ClassType.Toripon,
                Class.ClassType.Yumipon
            };
            //Serialize --
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
