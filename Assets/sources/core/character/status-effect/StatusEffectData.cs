using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character
{
    class StatusEffectData : MonoBehaviour
    {
        [SerializeField]
        GameObject _fireEffect;
        [SerializeField]
        GameObject _iceEffect;
        [SerializeField]
        GameObject _sleepEffect;
        [SerializeField]
        GameObject _bossFireEffect;
        [SerializeField]
        GameObject _bossIceEffect;

        private Dictionary<StatusEffectType, GameObject> _statusEffectMap;
        private Dictionary<StatusEffectType, GameObject> _bossStatusEffectMap;

        private void Awake()
        {
            _statusEffectMap = new Dictionary<StatusEffectType, GameObject>()
            {
                { StatusEffectType.Fire, _fireEffect },
                { StatusEffectType.Ice, _iceEffect },
                { StatusEffectType.Sleep, _sleepEffect }
            };
            _bossStatusEffectMap = new Dictionary<StatusEffectType, GameObject>()
            {
                { StatusEffectType.Fire, _bossFireEffect },
                { StatusEffectType.Ice, _bossIceEffect },
                { StatusEffectType.Sleep, _sleepEffect }
            };
        }
        public GameObject AttachEffect(StatusEffectType type, Transform body, bool isBigTarget)
        {
            var obj = Instantiate(
                (isBigTarget) ? _bossStatusEffectMap[type] : _statusEffectMap[type]
                , body);
            var renderer = obj.GetComponent<Renderer>();
            var targetObjectRenderer = body.GetComponentInChildren<SpriteRenderer>();
            renderer.sortingOrder = targetObjectRenderer.sortingOrder;
            renderer.sortingLayerID = targetObjectRenderer.sortingLayerID;
            return obj;
        }
    }
}
