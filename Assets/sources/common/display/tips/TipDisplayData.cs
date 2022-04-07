using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    [CreateAssetMenu(fileName = "tip-index", menuName = "Tip")]
    public class TipDisplayData : ScriptableObject
    {
        [SerializeField]
        int _index;
        public int Index => _index;
        [SerializeField]
        string _title;
        public string Title => _title;
        [TextArea]
        [SerializeField]
        string _content;
        public string Content => _content;
        [SerializeField]
        Sprite _image;
        public Sprite Image => _image;
        private void OnValidate()
        {
            if (_index < 0)
            {
                throw new System.ArgumentException("index cannot be less than zero");
            }
        }
    }
}
