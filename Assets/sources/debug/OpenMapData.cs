/*
using PataRoad.Core.Global;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.AppDebug
{
    public class OpenMapData : MonoBehaviour
    {
        [SerializeField]
        int _mapIndexToOpen;
        // Use this for initialization
        void Start()
        {
            var mapInfo = GlobalData.CurrentSlot.MapInfo;
            var closedList = typeof(MapInfo).GetField("_closedMaps",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(mapInfo) as List<int>;
            closedList.Remove(_mapIndexToOpen);
            mapInfo.OpenInIndex(_mapIndexToOpen);
        }
    }
}
*/