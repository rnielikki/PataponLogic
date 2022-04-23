using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    [System.Serializable]
    public class TipsCollection : ISerializationCallbackReceiver
    {
        static TipDisplayData[] _allTips;
        const string _tipsPath = "Tips/Content";
        private const int _defaultIndex = 0;
        [SerializeField]
        List<int> _openedTipIndexes = new List<int>();
        [System.NonSerialized]
        List<TipDisplayData> _openedTips;
        [System.NonSerialized]
        int _savedTipIndex = -1;

        public int OpenTipsCount => _openedTipIndexes.Count;
        public static int AllTipsCount => _allTips.Length;

        private TipsCollection()
        {
        }
        public static TipsCollection Create()
        {
            var tips = new TipsCollection();
            tips._openedTips = new List<TipDisplayData>();
            tips.GetTipInIndex(_defaultIndex);
            return tips;
        }
        /// <summary>
        /// Load all tips in very first (expected to be called in <see cref="Core.Global.GlobalData"/>).
        /// </summary>
        /// <exception cref="System.ArgumentException">If the tip display data have duplicated index.</exception>
        public static void LoadAllTips()
        {
            if (_allTips != null) return;
            var allTips = Resources.LoadAll<TipDisplayData>(_tipsPath);
            var maxIndex = allTips.Max(tip => tip.Index);
            _allTips = new TipDisplayData[maxIndex + 1];
            foreach (var tip in allTips)
            {
                //duplicated index checking
                if (_allTips[tip.Index] != null)
                {
                    throw new System.ArgumentException(
                        $"Duplicated tip index found in {tip.Index}: Check data {_allTips[tip.Index].Title} and {tip.Title}");
                }
                _allTips[tip.Index] = tip;
            }
        }
        /// <summary>
        /// Checks if a tip index is open.
        /// </summary>
        /// <param name="index">tip index to check if the corresponding tip is open.</param>
        /// <returns><c>true</c> if the tip from the index is already open, otherwise <c>false</c>.</returns>
        public bool HasTipIndex(int index) => _openedTipIndexes.Contains(index);
        /// <summary>
        /// Save tip index to use Tip display scene, expected to be called from before tip display scene.
        /// </summary>
        /// <param name="index">the index ot release.</param>
        public void SaveTipIndex(int index)
        {
            _savedTipIndex = index;
        }
        /// <summary>
        /// Open tip index to use Tip display scene, expected to be called from tip display scene.
        /// </summary>
        /// <returns>Tip display data in the corresponding saved index.</returns>
        public TipDisplayData ReleaseTip()
        {
            var tip = GetTipInIndex(_savedTipIndex);
            _savedTipIndex = -1;
            return tip;
        }
        /// <summary>
        /// Get Tip index. If doesn't exist in opened tip indexes, it ADDS to opened tips.
        /// </summary>
        /// <param name="index">Index of the tip.</param>
        /// <returns>The <see cref="TipDisplayData"/> in the corresponding index.</returns>
        private TipDisplayData GetTipInIndex(int index)
        {
            if (index < 0 || index >= _allTips.Length || _allTips[index] == null)
            {
                return GetRandomTip();
            }
            if (!_openedTipIndexes.Contains(index))
            {
                _openedTipIndexes.Add(index);
                _openedTips.Add(_allTips[index]);
            }
            return _allTips[index];
        }
        private TipDisplayData GetRandomTip()
        {
            return _openedTips[Random.Range(0, _openedTips.Count)];
        }
        /// <summary>
        /// Loads all opened tips. Can be used for initialisation, or for just listing open tip indexes.
        /// </summary>
        /// <returns>Currently opened tips in the game save, in the index order.</returns>
        public IEnumerable<TipDisplayData> GetAllOpenedTips()
            => _openedTips.OrderBy(tip => tip.Index);
        private void LoadOpenedTips()
        {
            _openedTips = new List<TipDisplayData>();
            foreach (var index in _openedTipIndexes)
            {
                _openedTips.Add(_allTips[index]);
            }
        }

        public void OnBeforeSerialize()
        {
            //eeh...
        }

        public void OnAfterDeserialize()
        {
            LoadOpenedTips();
        }
    }
}