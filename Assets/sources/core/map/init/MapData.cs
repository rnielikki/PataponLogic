using UnityEngine;

namespace PataRoad.Core.Map
{
    public class MapData : ScriptableObject
    {
        [SerializeField]
        private string _defaultMusic;
        public string DefaultMusic => _defaultMusic;
        [SerializeField]
        private string _background;
        public string DefaultBackgroundImage => _background;
        [SerializeField]
        private int _maxBossSummonCount;
        public int SummonCount => _maxBossSummonCount;
        [SerializeField]
        GameObject _content;
        public GameObject Content => _content;
    }
}
