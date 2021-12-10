using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Data
{
    [System.Serializable]
    public class PataponClassInfo : ISerializationCallbackReceiver
    {
        [SerializeField]
        Class.ClassType _class;
        public Class.ClassType Class => _class;
        [SerializeReference]
        PataponEquipmentInfo[] _info;

        private Items.GeneralModeData _generalModeData;
        [SerializeField]
        private int _generalModeDataIndex;

        internal PataponClassInfo(Class.ClassType type)
        {
            _class = type;
            _info = new PataponEquipmentInfo[4]
            {
                new PataponEquipmentInfo(type, true),
                new PataponEquipmentInfo(type, false),
                new PataponEquipmentInfo(type, false),
                new PataponEquipmentInfo(type, false)
            };
        }
        internal IEnumerable<Items.EquipmentData> GetAllEquipments() => _info.SelectMany(info => info.GetAllEquipments());
        internal IEnumerable<Items.EquipmentData> GetEquipmentInIndex(int index)
        {
            if (index > 4 || index < 0)
            {
                throw new System.ArgumentException($"Index (in group) {index} is not valid so equipment info cannot be returned");
            }
            return _info[index].GetAllEquipments();
        }
        internal void SetEquipmentInIndex(int index, Items.EquipmentData data)
        {
            if (index > 4 || index < 0)
            {
                throw new System.ArgumentException($"Index (in group) {index} is not valid so cannot equip.");
            }
            _info[index].Equip(data);
        }
        internal void SetGeneralModeData(Items.GeneralModeData data)
        {
            _generalModeData = data;
        }
        internal List<int> GetAllHolders(Items.EquipmentData data)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (_info[i].HasEquipment(data)) res.Add(i);
            }
            return res;
        }

        public void OnBeforeSerialize()
        {
            _generalModeDataIndex = _generalModeData?.Index ?? -1;
        }

        public void OnAfterDeserialize()
        {
            if (_generalModeDataIndex != -1) _generalModeData = Items.ItemLoader.GetItem<Items.GeneralModeData>(Items.ItemType.Key, "GeneralMode", _generalModeDataIndex);
        }
    }
}
