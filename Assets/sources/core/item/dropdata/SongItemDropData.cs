using UnityEngine;

namespace PataRoad.Core.Items
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "song-item-drop", menuName = "ItemDrop/Song")]
    public class SongItemDropData : ObtainableItemDropData
    {
        [SerializeField]
        GameObject _tutorialScreen;
        [SerializeField]
        string _startMessage;
        public string StartMessage => _startMessage;
        [SerializeField]
        string[] _processingMessage = new string[Rhythm.Command.PracticingCommandListData.FullPracticeCount];
        public string[] ProcessingMessage => _processingMessage;
        [SerializeField]
        string _endMessage;
        [SerializeField]
        AudioClip _teachingSound;
        public AudioClip TeachingSound => _teachingSound;
        public string EndMessage => _endMessage;
        private Rhythm.Command.CommandSong _song;
        public override UnityEngine.Events.UnityEvent Events
        {
            get
            {
                var ev = new UnityEngine.Events.UnityEvent();
                var song = (Item as SongItemData)?.Song;
                if (song != null && song.Value != Rhythm.Command.CommandSong.None)
                {
                    _song = song.Value;
                    ev.AddListener(Open);
                }
                return ev;
            }
        }
        public UnityEngine.Events.UnityEvent OnSongComplete { get; private set; } = new UnityEngine.Events.UnityEvent();
        private void Open()
        {
            var screen = Instantiate(_tutorialScreen).GetComponent<Common.GameDisplay.SongTutorial>();
            screen.Init(_song, this);
        }
        public void EndPractice() => OnSongComplete?.Invoke();
        void OnValidate()
        {
            if (_processingMessage.Length != Rhythm.Command.PracticingCommandListData.FullPracticeCount)
            {
                Debug.LogError($"[SongItemDropData {name}] Set the processing message length to {Rhythm.Command.PracticingCommandListData.FullPracticeCount}, but it's {_processingMessage.Length}");
            }
        }
    }
}
