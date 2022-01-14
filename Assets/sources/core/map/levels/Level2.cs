using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level2 : MonoBehaviour
    {
        private void Start()
        {
            foreach (var character in GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Character.Patapons.Patapon>())
            {
                character.CreateAfterDeathEvent().AddListener(() => MissionPoint.Current.WaitAndFailMission(4));
            }
        }
    }
}
