namespace PataRoad.Core.Map.Levels
{
    class Level7 : UnityEngine.MonoBehaviour
    {
        private void Start()
        {

            MissionPoint.Current.AddMissionEndAction((success) =>
            {
                if (success)
                {
                    var progress = Global.GlobalData.CurrentSlot.Progress;
                    progress.IsGemMinigameOpen = true;
                }
            });
        }
    }
}
