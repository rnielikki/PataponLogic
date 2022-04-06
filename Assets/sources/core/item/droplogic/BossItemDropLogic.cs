using static PataRoad.Common.Utils;

namespace PataRoad.Core.Items
{
    internal class BossItemDropLogic : ILeveldItemDropLogic
    {
        private string _itemGroup;
        public void SetItemGroup(string group) => _itemGroup = group;
        public int GetAmount() => (int)Rhythm.RhythmEnvironment.Difficulty + 1;
        readonly float[] _levelSlopeMap;
        private int _levelGroup;
        public BossItemDropLogic(int levelGroup)
        {
            switch (levelGroup)
            {
                case 0:
                    _levelSlopeMap = new float[]
                    {
                        0.5f,
                        0.4f,
                        0.2f,
                        0
                    };
                    break;
                case 1:
                    _levelSlopeMap = new float[]
                    {
                        1,
                        1,
                        0.8f,
                        0.7f
                    };
                    break;
                case 2:
                    _levelSlopeMap = new float[]
                    {
                        0.2f,
                        0.4f,
                        0f,
                        0
                    };
                    break;
                default:
                    throw new System.ArgumentException(
                        $"For boss, the level group ('MinLevel' in {nameof(LeveledItemDropBehvaiour)}) must be range of 0-2."
                    );
            }
            _levelGroup = levelGroup;
        }
        public IItem GetItem(float levelRatio)
        {
            int itemIndex = 4;
            if (RandomByProbability(GetProbability(levelRatio, 0.5f, _levelSlopeMap[0], false)))
            {
                itemIndex = 0;
            }
            else if (RandomByProbability(GetProbability(levelRatio, 0.7f, _levelSlopeMap[1], false)))
            {
                itemIndex = 1;
            }
            else if (RandomByProbability(
                (levelRatio <= 0.5f || _levelGroup != 1) ? GetProbability(levelRatio, 1, _levelSlopeMap[2], true) :
                GetProbability(levelRatio - 0.5f, 0.6f, _levelSlopeMap[2], false)
                ))
            {
                itemIndex = 2;
            }
            else if (RandomByProbability(
                GetProbability(levelRatio, 1, _levelSlopeMap[3], true)
                ))
            {
                itemIndex = 3;
            }
            if (_levelGroup == 2) itemIndex += 2;
            return ItemLoader.GetItem(ItemType.Material, _itemGroup, itemIndex);
        }
        private float GetProbability(float levelRatio, float firstValue, float slope, bool ascending)
        {
            if (!ascending) slope *= -1;
            return firstValue + levelRatio * slope;
        }
    }
}