using static PataRoad.Common.Utils;

namespace PataRoad.Core.Items
{
    internal class BossItemDropLogic : ILeveldItemDropLogic
    {
        private string _itemGroup;
        public void SetItemGroup(string group) => _itemGroup = group;
        public int GetAmount() => (int)Rhythm.RhythmEnvironment.Difficulty + 1;
        readonly float[] _levelSlopeMap = new float[]
        {
            0.5f,
            0.5f,
            0.4f,
            0.35f
        };
        public BossItemDropLogic(int levelGroup)
        {
            if (levelGroup < 0 || levelGroup > 2)
            {
                throw new System.ArgumentException(
                    $"For boss, the level group ('MinLevel' in {nameof(LeveledItemDropBehvaiour)}) must be range of 0-2."
                );
            }
            for (int i = 0; i < _levelSlopeMap.Length; i++)
            {
                _levelSlopeMap[i] *= (levelGroup + 1);
            }
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
                (levelRatio <= 0.5f) ? GetProbability(levelRatio, 1, _levelSlopeMap[2], true) :
                GetProbability(levelRatio - 0.5f, 0.6f, 0.8f, false)
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
            return ItemLoader.GetItem(ItemType.Material, _itemGroup, itemIndex);
        }
        private float GetProbability(float levelRatio, float firstValue, float slope, bool ascending)
        {
            if (!ascending) slope *= -1;
            return firstValue + levelRatio * slope;
        }
    }
}