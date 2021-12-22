using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis
{
    public class MaterialSummaryElement : SummaryElement
    {
        private Core.Items.IItem _current;
        [SerializeField]
        private Image _thumbnailImage;
        public Core.Items.IItem Item => _current;

        void Awake()
        {
            Init();
        }

        public void SetItem(Core.Items.IItem item)
        {
            _current = item;
            if (item != null)
            {
                _text.text = item.Name;
                _thumbnailImage.sprite = item.Image;
            }
            else
            {
                _text.text = "None";
            }
        }
    }
}
