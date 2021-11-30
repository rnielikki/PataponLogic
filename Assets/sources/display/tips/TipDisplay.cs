using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.GameDisplay
{
    public class TipDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _titleText;
        [SerializeField]
        private UnityEngine.UI.Text _contentText;
        [SerializeField]
        private UnityEngine.UI.Image _image;
        private static Dictionary<int, TipDisplayData> _allTipsIndex;
        private static TipDisplayData[] _allTips;
        // Start is called before the first frame update
        void Start()
        {
#pragma warning disable S2696 // Instance members should not write to "static" fields
            if (_allTipsIndex == null)
            {
                _allTips = Resources.LoadAll<TipDisplayData>("Tips/Content");
                _allTipsIndex = new Dictionary<int, TipDisplayData>();
                foreach (var tip in _allTips)
                {
                    if (int.TryParse(tip.name, out int index))
                    {
                        _allTipsIndex.Add(index, tip);
                    }
                }
            }
#pragma warning restore S2696 // Instance members should not write to "static" fields

            var tipindex = FindObjectOfType<Core.GlobalData>().TipIndex;
            if (tipindex > -1 && _allTipsIndex.TryGetValue(tipindex, out TipDisplayData data))
            {
                LoadTip(data);
            }
            else
            {
                LoadTip(_allTips[Random.Range(0, _allTips.Length - 1)]);
            }
        }
        private void LoadTip(TipDisplayData data)
        {
            _titleText.text = data.Title;
            _contentText.text = data.Content;
            if (data.Image != null)
            {
                _image.sprite = data.Image;
                _image.enabled = true;
            }
            else
            {
                _image.enabled = false;
            }
        }
    }
}
