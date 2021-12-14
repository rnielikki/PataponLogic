using PataRoad.Core.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Global
{
    [System.Serializable]
    public class MapInfo : IPlayerData
    {
        public MapData NextMapData { get; private set; }
        public MapData LastMapData { get; private set; }
        private List<MapData> _openMaps = new List<MapData>();

        [SerializeField]
        private int _nextMapIndex;
        [SerializeField]
        private bool _succeededLast;
        public bool SuccededLast => _succeededLast;
        [SerializeField]
        private int[] _openMapIndexes;
        public const string MapPath = "Map/Levels/";

        //-- boss
        //----------- WE'LL NEED TO ADD THIS!
        private Dictionary<MapData, int> _bossLevels = new Dictionary<MapData, int>();
        private const int _maxBossLevel = 99;

        internal MapInfo()
        {
            NextMapData = LoadResource(2);
        }
        public void OpenNext()
        {
            if (NextMapData.NextIndex > 0)
            {
                NextMapData = LoadResource(NextMapData.NextIndex);
            }
        }
        public void MissionSucceeded()
        {
            if (_bossLevels.ContainsKey(NextMapData) && _bossLevels[NextMapData] < _maxBossLevel)
            {
                _bossLevels[NextMapData]++;
            }
            LastMapData = NextMapData;
            OpenNext();

            _succeededLast = true;
            GlobalData.PataponInfo.CustomMusic = null;
        }
        public void MissionFailed()
        {
            _succeededLast = false;
            GlobalData.PataponInfo.CustomMusic = null;
        }
        private MapData LoadResource(int index)
        {
            var mapData = Resources.Load<MapData>(MapPath + index.ToString());
            if (mapData == null) return null;
            if (!_openMaps.Contains(mapData))
            {
                mapData.Index = index;
                _openMaps.Add(mapData);
            }
            return mapData;
        }
        public string Serialize()
        {
            _nextMapIndex = NextMapData.Index;
            _openMapIndexes = _openMaps.Select(map => map.Index).ToArray();
            return JsonUtility.ToJson(this);
        }
        public void Deserialize()
        {
            foreach (var mapIndex in _openMapIndexes)
            {
                var res = LoadResource(mapIndex);
                if (mapIndex == _nextMapIndex)
                {
                    NextMapData = res;
                }
            }
            if (NextMapData == null)
            {
                NextMapData = _openMaps.Last();
            }
        }
    }
}
