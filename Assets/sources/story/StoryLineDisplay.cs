using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Story
{
    internal class StoryLineDisplay : MonoBehaviour
    {
        [SerializeField]
        Image _image;
        [SerializeField]
        Text _name;
        [SerializeField]
        Text _content;
        [SerializeField]
        Common.GameDisplay.ScrollWindow _scrollWindow;
        public System.Collections.IEnumerator WaitUntilNext(StoryAction story)
        {
            gameObject.SetActive(true);
            UpdateText(story);
            yield return new WaitUntil(() => !gameObject.activeSelf);
        }
        private void UpdateText(StoryAction story)
        {
            gameObject.SetActive(true);
            _image.enabled = story.Image != null;
            _image.sprite = story.Image;
            _name.text = story.Name;
            _content.text = FormatContent(story.Content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content.rectTransform);
            _scrollWindow.Refresh();
        }
        private string FormatContent(string original)
        {
            return original.Replace("%Almighty%", Core.Global.GlobalData.CurrentSlot.AlmightyName);
        }
        public void Close()
        {
            _image.enabled = false;
            gameObject.SetActive(false);
        }
    }
}