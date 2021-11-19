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
        string _processingMessage;
        public string ProcessingMessage => _processingMessage;
        [SerializeField]
        string _endMessage;
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
        private void Open()
        {
            var screen = Instantiate(_tutorialScreen).GetComponent<GameDisplay.SongTutorial>();
            screen.Init(_song, this);
        }
    }
}
