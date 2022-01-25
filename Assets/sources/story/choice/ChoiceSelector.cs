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

            StoryAction[] actions = null;
            foreach (var item in items)
            {
                item.Button.onClick.AddListener(() =>
                {
                    actions = item.StoryActions;
                    Close();
                });
            }
            yield return new WaitUntil(() => actions != null);
            yield return storySceneInfo.LoadStoryLines(actions);
        }
        private void Close() => gameObject.SetActive(false);
    }
}
