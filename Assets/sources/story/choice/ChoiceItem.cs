using UnityEngine;

namespace PataRoad.Story
{
    class ChoiceItem : MonoBehaviour
    {
        [SerializeField]
        private StoryAction[] _storyActions;
        internal StoryAction[] StoryActions => _storyActions;
        [SerializeField]
        private UnityEngine.UI.Button _button;
        internal UnityEngine.UI.Button Button => _button;
    }
}
