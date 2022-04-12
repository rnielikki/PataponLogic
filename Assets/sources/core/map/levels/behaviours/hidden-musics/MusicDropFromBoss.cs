namespace PataRoad.Core.Map.Levels.MusicDrops
{
    public class MusicDropFromBoss : AutoMusicDrop
    {
        [UnityEngine.SerializeField]
        int _levelToDrop;

        void Start()
        {
            var boss = GetComponent<Character.Bosses.Boss>();
            if (_levelToDrop <= boss.GetLevel())
            {
                boss.OnAfterDeath.AddListener(DropMusic);
            }
        }
    }
}