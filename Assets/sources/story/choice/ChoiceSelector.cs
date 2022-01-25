using UnityEngine;

namespace PataRoad.Story
{
    class ChoiceSelector : MonoBehaviour
    {
        public System.Collections.IEnumerator Open(StorySceneInfo storySceneInfo)
        {
            gameObject.SetActive(true);
            ChoiceItem[] items = GetComponentsInChildren<ChoiceItem>();
            items[0].Button.Select();

            ChoiceItem selected = null;
            foreach (var item in items)
            {
                item.Button.onClick.AddListener(() =>
                {
                    selected = item;
                    Close();
                });
            }
            yield return new WaitUntil(() => selected != null);
            yield return storySceneInfo.LoadStoryLines(selected.StoryActions, selected.ChoiceSelector, selected.NextStory);
        }
        private void Close() => gameObject.SetActive(false);
    }
}
