using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [System.Serializable]
    public class PataponClassInfo
    {
        [SerializeField]
        Class.ClassType _class;
        public Class.ClassType Class => _class;
        [SerializeReference]
        PataponEquipmentInfo[] _info;
        internal PataponClassInfo(Class.ClassType type)
        {
            _class = type;
            _info = new PataponEquipmentInfo[4]
            {
                new PataponEquipmentInfo(type),
                new PataponEquipmentInfo(type),
                new PataponEquipmentInfo(type),
                new PataponEquipmentInfo(type)
            };
        }
        public PataponEquipmentInfo GetEquipmentInIndex(int index)
        {
            if (index > 4 || index < 0)
            {
                throw new System.ArgumentException($"Index (in group) {index} is not valid so equipment info cannot be returned");
            }
            return _info[index];
        }
    }
}
