﻿using PataRoad.SceneLogic.Patapolis.ItemExchange;
using UnityEngine;

namespace PataRoad.Core.Global
{
    [System.Serializable]
    public class ExchangeRateData
    {
        [SerializeField]
        float _alloyRate = _alloyRatio;
        const float _alloyRatio = 0.1f;
        [SerializeField]
        float _meatRate = _meatRatio;
        const float _meatRatio = 0.15f;
        [SerializeField]
        float _liquidRate = _liquidRatio;
        const float _liquidRatio = 0.2f;
        [SerializeField]
        float _hideRate = _hideRatio;
        const float _hideRatio = 0.04f;
        [SerializeField]
        float _seedRate = _seedRatio;
        const float _seedRatio = 0.2f;
        public void RaiseRate(FromMaterialType materialType, int level)
        {
            switch (materialType)
            {
                case FromMaterialType.Mineral:
                    _alloyRate += _alloyRatio * (level + 1);
                    return;
                case FromMaterialType.Bone:
                    _meatRate += _meatRatio * (level + 1);
                    return;
                case FromMaterialType.Tree:
                    _liquidRate += _liquidRatio * (level + 1);
                    return;
                case FromMaterialType.Fang:
                    _hideRate += _hideRatio * (level + 1);
                    return;
                case FromMaterialType.Vegetable:
                    _seedRate += _seedRatio * (level + 1);
                    return;
                default:
                    throw new System.NotImplementedException("material type not supported");
            }
        }
        public float GetRate(FromMaterialType materialType)
            => materialType switch
            {
                FromMaterialType.Mineral => _alloyRate,
                FromMaterialType.Bone => _meatRate,
                FromMaterialType.Tree => _liquidRate,
                FromMaterialType.Fang => _hideRate,
                FromMaterialType.Vegetable => _seedRate,
                _ => throw new System.NotImplementedException("material type not supported")
            };
        public int GetAmount(FromMaterialType materialType)
            => (int)(GetRate(materialType)) + 1;
    }
}