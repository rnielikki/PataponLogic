using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level9 : MonoBehaviour
    {
        [SerializeField]
        Common.GameDisplay.MiracleTutorial _miracleScreen;
        [SerializeField]
        [TextArea]
        string _onFailureText;
        private void Start()
        {
            Character.Patapons.PataponsManager.AllowedToGoForward = false;
            var donDrum = Items.ItemLoader.GetItem(Items.ItemType.Key, "Drum", 3);

            if (!Global.GlobalData.CurrentSlot.Inventory.HasItem(donDrum)
                ||
                !FindObjectOfType<Character.Bosses.BossSummonManager>().HasBoss)
            {
                _miracleScreen.SetFailureText(_onFailureText);
                MissionPoint.Current.WaitAndFailMission(8);
            }
            else
            {
                _miracleScreen.StartTutorial();
                _miracleScreen.OnSongComplete
                    .AddListener(() => Character.Patapons.PataponsManager.AllowedToGoForward = true);
            }
        }
    }
}
