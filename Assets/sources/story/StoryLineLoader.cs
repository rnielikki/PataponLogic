using UnityEngine;

namespace PataRoad.Story
{
    /// <summary>
    /// Loads simply only storylines without any scene or other environment change.
    /// </summary>
    class StoryLineLoader : MonoBehaviour
    {
        [Header("Story Data")]
#pragma warning disable S1104 // Fields should not have public accessibility - we need AGAIN this for serialized array in inspector.
        public StoryAction[] StoryActions;
#pragma warning restore S1104 // Fields should not have public accessibility
        [SerializeField]
        StorySceneInfo _storySceneInfo;
        [UnityEngine.SerializeField]
        ChoiceSelector _choiceSelector;
        internal ChoiceSelector ChoiceSelector => _choiceSelector;
        [UnityEngine.SerializeField]
        StoryData _nextStory;
        internal StoryData NextStory => _nextStory;

        public void ReadLines() => StartCoroutine(_storySceneInfo.LoadStoryLines(StoryActions, _choiceSelector, _nextStory));
    }
}
