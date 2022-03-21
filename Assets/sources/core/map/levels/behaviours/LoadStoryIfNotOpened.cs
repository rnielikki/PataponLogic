using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class LoadStoryIfNotOpened : MonoBehaviour
    {
        [SerializeField]
        int _nextMapIndex;
        [SerializeField]
        string _nextStoryName;
        // Use this for initialization
        void Start()
        {
            if (!Global.GlobalData.CurrentSlot.MapInfo.HasMapOpenedYet(_nextMapIndex))
            {
                MissionPoint.Current.NextStory = _nextStoryName;
            }
        }
    }
}