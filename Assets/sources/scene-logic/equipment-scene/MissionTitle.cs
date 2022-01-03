﻿using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class MissionTitle : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI _text;
        private void Start()
        {
            _text.text = Core.Global.GlobalData.CurrentSlot.MapInfo.NextMap?.GetNameWithLevel() ?? "[NO MAP DATA]";
        }
    }
}
