using UnityEngine;

namespace PataRoad.Story
{
    class ChoiceSelector : MonoBehaviour
    {
        ChoiceItem[] _items;

        public void Open(StorySceneInfo storySceneInfo)
        {
            gameObject.SetActive(true);
            _items = GetComponentsInChildren<ChoiceItem>();
            _items[0].Button.Select();
            foreach (var item in _items)
            {
                item.Button.onClick.AddListener(() =>
                StartCoroutine(storySceneInfo.LoadStoryLines(item.StoryActions)));
            }
        }
    }
}
