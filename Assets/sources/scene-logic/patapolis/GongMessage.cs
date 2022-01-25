using PataRoad.Story;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class GongMessage : MonoBehaviour
    {
        const int _minProgress = 7;
        [System.Serializable]
        private class StoryActionByProgressIndexWrapper
        {
            [SerializeField]
            StoryActionByProgressIndex[] _lines;
            public StoryActionByProgressIndex[] Lines => _lines;
        }
        [System.Serializable]
        private class StoryActionByProgressIndex
        {
            [SerializeField]
            Sprite _image;
            internal Sprite Image => _image;
            [SerializeField]
            [TextArea]
            string _content;
            internal string Content => _content;
        }
        [SerializeField]
        StorySceneInfo _storySceneInfo;
        [Header("Story Data")]
        [SerializeField]
        private StoryAction DefaultStoryActionTemplate;
        [SerializeField]
        private StoryAction LastStoryActionTemplate;
        [SerializeField]
        StoryActionByProgressIndexWrapper[] _storyByProgress;

        private StoryAction[] _currentActions;
        private void Start()
        {
            _currentActions = LoadStoryActions();
        }
        public void ReadLines() => StartCoroutine(_storySceneInfo.LoadStoryLines(_currentActions, null, null));

        private StoryAction[] LoadStoryActions()
        {
            var progress = Core.Global.GlobalData.CurrentSlot.MapInfo.Progress - _minProgress;
            if (progress < 0)
            {
                var action1 = DefaultStoryActionTemplate.Copy();
                var action2 = DefaultStoryActionTemplate.Copy();
                action1.Content = "IndexOutOfRangeException: Value cannot be less than zero.";
                action2.Content = "What the heck did you thinking? Are we hacked? Go away! I don't want to talk to you!";
                return new StoryAction[] { action1, action2, LastStoryActionTemplate };
            }
            else if (progress >= _storyByProgress.Length)
            {
                return new StoryAction[] { DefaultStoryActionTemplate, LastStoryActionTemplate };
            }
            else
            {
                var lines = _storyByProgress[progress].Lines;
                var result = new StoryAction[lines.Length + 1];
                for (int i = 0; i < lines.Length; i++)
                {
                    var copied = DefaultStoryActionTemplate.Copy();
                    copied.Image = lines[i].Image;
                    copied.Content = lines[i].Content;
                    result[i] = copied;
                }
                result[lines.Length] = LastStoryActionTemplate;
                return result;
            }
        }
    }
}
