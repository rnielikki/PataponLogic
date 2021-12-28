namespace PataRoad.Story
{
    [System.Serializable]
    public class StoryAction
    {
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
        public UnityEngine.Sprite Image => _image;
        [UnityEngine.SerializeField]
        private string _name;
        public string Name => _name;
        [UnityEngine.SerializeField]
        [UnityEngine.TextArea]
        private string _content;
        public string Content => _content;
        internal void InvokeEvent() => _events?.Invoke();
    }
}
