using PataRoad.SceneLogic.Patapolis.ItemExchange;
using UnityEngine;

namespace PataRoad.Core.Global
{
    [System.Serializable]
    public class ExchangeRateData
    {
        private const float _maxRate = 20;
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
                    if (_alloyRate < _maxRate)
                    {
                        _alloyRate += _alloyRatio * (level + 1);
                    }
                    return;
                case FromMaterialType.Bone:
                    if (_meatRate < _maxRate)
                    {
                        _meatRate += _meatRatio * (level + 1);
                    }
                    return;
                case FromMaterialType.Tree:
                    if (_liquidRate < _maxRate)
                    {
                        _liquidRate += _liquidRatio * (level + 1);
                    }
                    return;
                case FromMaterialType.Fang:
                    if (_hideRate < _maxRate)
                    {
                        _hideRate += _hideRatio * (level + 1);
                    }
                    return;
                case FromMaterialType.Vegetable:
                    if (_seedRate < _maxRate)
                    {
                        _seedRate += _seedRatio * (level + 1);
                    }
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