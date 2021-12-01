/*
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [System.Serializable]
    public class PataponCurrentClassInfo
    {
        [SerializeField]
        Class.ClassType _class;
        public Class.ClassType Class => _class;
        [SerializeReference]
        PataponClassEquipmentInfo[] _info;
        internal PataponCurrentClassInfo(Class.ClassType type)
        {
            _class = type;
            _info = new PataponClassEquipmentInfo[4]
            {
                new PataponClassEquipmentInfo(),
                new PataponClassEquipmentInfo(),
                new PataponClassEquipmentInfo(),
                new PataponClassEquipmentInfo()
            };
        }
        public PataponClassEquipmentInfo GetEquipmentInIndex(int index)
        {
            if (index > 4 || index < 0)
            {
                throw new System.ArgumentException($"Index (in group) {index} is not valid so equipment info cannot be returned");
            }
            return _info[index];
        }
    }
}
*/
