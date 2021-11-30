using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    [CreateAssetMenu(fileName = "tip-index", menuName = "Tip")]
    class TipDisplayData : ScriptableObject
    {
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
    }
}
