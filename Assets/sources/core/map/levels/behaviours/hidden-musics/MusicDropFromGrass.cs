namespace PataRoad.Core.Map.Levels.MusicDrops
{
    public class MusicDropFromGrass : AutoMusicDrop
    {
        // Use this for initialization
        void Start()
        {
            GetComponent<Character.Grass>().OnDeadEvent
                .AddListener(DropMusic);
        }
    }
}