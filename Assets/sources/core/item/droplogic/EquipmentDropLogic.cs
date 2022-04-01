using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Items
{
    internal class EquipmentDropLogic : ILeveldItemDropLogic
    {
        private readonly int _minLevel;
        private readonly int _maxLevel;
        private readonly float _startGap;

        private readonly List<string> _availableItems = new List<string>();

        private readonly float _chanceToDrop;

        internal EquipmentDropLogic(int minLevel, int maxLevel)
        {
            _minLevel = minLevel;
            _maxLevel = maxLevel;
            _startGap = 0.5f / (maxLevel - minLevel);
            switch (Rhythm.RhythmEnvironment.Difficulty)
            {
                case Rhythm.Difficulty.Easy:
                    _chanceToDrop = 0.05f;
                    break;
                case Rhythm.Difficulty.Normal:
                    _chanceToDrop = 0.15f;
                    break;
                case Rhythm.Difficulty.Hard:
                    _chanceToDrop = 0.25f;
                    break;
            }
        }
        public int GetAmount() => 1;

        public IItem GetItem(float levelRatio)
        {
            if (!Common.Utils.RandomByProbability(_chanceToDrop)) return null;
            var group = _availableItems[Random.Range(0, _availableItems.Count)];

            for (int i = _minLevel; i < _maxLevel; i++)
            {
                float probability = (0.5f + i * _startGap) - i * _startGap * levelRatio;
                if (Common.Utils.RandomByProbability(probability))
                {
                    return GetItemFromIndex(i);
                }
            }
            return GetItemFromIndex(_maxLevel);
            IItem GetItemFromIndex(int index) => ItemLoader.GetItem(ItemType.Equipment, group, index);
        }

        public void SetItemGroup(string group)
        {
            var classes =
                Global.GlobalData.CurrentSlot.Inventory.GetItemsByType(ItemType.Key, "Class");

            foreach (var @class in classes)
            {
                var classType = (@class.Item as ClassMemoryData).Class;
                (string weapon, string protector) =
                    Character.Class.ClassAttackEquipmentData.GetWeaponAndProtectorName(classType);
                if (weapon != null) _availableItems.Add(weapon);
                if (protector != null) _availableItems.Add(protector);
            }
            _availableItems.Add("Helm");
            //item group should be decided by...
        }
    }
}