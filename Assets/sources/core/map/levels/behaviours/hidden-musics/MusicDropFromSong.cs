namespace PataRoad.Core.Map.Levels.MusicDrops
{
    public class MusicDropFromSong : AutoMusicDrop
    {
        [UnityEngine.SerializeField]
        Rhythm.Command.CommandSong _song;
        bool _done;
        void Start()
        {
            FindObjectOfType<Rhythm.Command.RhythmCommand>()
                .OnCommandInput.AddListener(DropMusicIfNotDropped);
        }
        void DropMusicIfNotDropped(Rhythm.Command.RhythmCommandModel model)
        {
            if (!_done && model.Song == _song
                && model.ComboType == Rhythm.Command.ComboStatus.Fever)
            {
                DropMusic(Character.Patapons.PataponsManager.Current.transform.position);
                _done = true;
            }
        }
    }
}