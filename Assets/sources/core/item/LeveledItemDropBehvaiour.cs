using UnityEngine;

namespace PataRoad.Core.Items
{
    public class LeveledItemDropBehvaiour : ItemDropBehaviour, Map.IHavingLevel
    {
        [SerializeField]
        LeveledItemDropType _type;
        [SerializeField]
        int _minItemLevel;
        [SerializeField]
        int _maxItemLevel;
        [SerializeField]
        string _itemGroupName;

        float _targetLevelRatio;
        private ILeveldItemDropLogic _logic;

        private ItemDropData[] _overridenDropData;
        public override ItemDropData[] DropData => _overridenDropData;

        private void Awake()
        {
            _logic = LeveledItemDrop.GetLogic(_type, _minItemLevel, _maxItemLevel);
            _logic.SetItemGroup(_itemGroupName);
            LoadDropData();
        }
        public override void Drop()
        {
            var startPoint = transform.position;
            startPoint.x -= DropData.Length * _gap * 0.5f;
            foreach (var data in DropData)
            {
                var item = _logic.GetItem(_targetLevelRatio); //random every time
                if (item != null)
                {
                    ItemDrop.DropItem(data, startPoint, data.DoNotDestroy, item);
                    startPoint.x += _gap;
                }
            }
        }
        private void LoadDropData()
        {
            var amount = _logic.GetAmount();
            _overridenDropData = new ItemDropData[amount];
            for (int i = 0; i < amount; i++)
            {
                _overridenDropData[i] = _dropData[0];
            }
        }
        public void SetLevel(int level, int absoluteMaxLevel)
        {
            _targetLevelRatio = (float)level / absoluteMaxLevel;
        }
    }
}