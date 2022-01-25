namespace PataRoad.Story
{
    [System.Serializable]
    public class StoryAction
    {
        [UnityEngine.SerializeField]
        ChoiceSelector _choiceSelector;
        internal ChoiceSelector ChoiceSelector => _choiceSelector;
        [UnityEngine.SerializeField]
        StoryData _nextStory;
        internal StoryData NextStory => _nextStory;

        [UnityEngine.SerializeField]
        UnityEngine.Events.UnityEvent _events;
        [UnityEngine.SerializeField]
        float _waitingSeconds;
        public float WaitingSeconds => _waitingSeconds;
        [UnityEngine.Header("Line")]
        [UnityEngine.SerializeField]
        private bool _useLine;
        public bool UseLine => _useLine;
        [UnityEngine.SerializeField]
        private UnityEngine.Sprite _image;
        public UnityEngine.Sprite Image
        {
            get => _image;
            set => _image = value;
        }
        [UnityEngine.SerializeField]
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        [UnityEngine.SerializeField]
        [UnityEngine.TextArea]
        private string _content;
        public string Content
        {
            get => _content;
            set => _content = value;
        }
        internal void InvokeEvent() => _events?.Invoke();
        public StoryAction Copy() => (StoryAction)MemberwiseClone();
    }
}
