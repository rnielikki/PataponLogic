using UnityEngine;

namespace PataRoad.Story
{
    public class RightChoiceMapOpen : MonoBehaviour
    {
        [SerializeField]
        int _mapIndex;
        public void RewardWithNewMap()
        {
            Core.Global.GlobalData.CurrentSlot.MapInfo.OpenAndSetAsNext(_mapIndex);
        }
    }
}