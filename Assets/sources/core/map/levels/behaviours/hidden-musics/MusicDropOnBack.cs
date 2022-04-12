using UnityEngine;

namespace PataRoad.Core.Map.Levels.MusicDrops
{
    public class MusicDropOnBack : AutoMusicDrop
    {
        private void Start()
        {
            var pos = Character.Patapons.PataponEnvironment.GroupDistance * 3
                + 0.5f * Character.CharacterEnvironment.DodgeDistance;
            DropMusic(pos * Vector3.left);
        }
    }
}