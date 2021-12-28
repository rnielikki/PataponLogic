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
        public System.Collections.IEnumerator WaitUntilNext(StoryAction story)
        {
            gameObject.SetActive(true);
            UpdateText(story);
            yield return new WaitUntil(() => !gameObject.activeSelf);
        }
        private void UpdateText(StoryAction story)
        {
            gameObject.SetActive(true);
            _image.sprite = story.Image;
            _name.text = story.Name;
            _content.text = story.Content;
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}