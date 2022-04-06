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

        public Dictionary<StatusEffectType, GameObject> GetStatusEffectMap(Transform body)
        {
            return new Dictionary<StatusEffectType, GameObject>()
            {
                { StatusEffectType.Fire, LoadEffect(_fireEffect, body) },
                { StatusEffectType.Ice, LoadEffect(_iceEffect, body) },
                { StatusEffectType.Sleep, LoadEffect(_sleepEffect, body) }
            };
        }
        public Dictionary<StatusEffectType, GameObject> GetBossStatusEffectMap(Transform body)
        {
            return new Dictionary<StatusEffectType, GameObject>()
            {
                { StatusEffectType.Fire, LoadEffect(_bossFireEffect, body) },
                { StatusEffectType.Ice, LoadEffect(_bossIceEffect, body) },
                { StatusEffectType.Sleep, LoadEffect(_sleepEffect, body) }
            };
        }
        private GameObject LoadEffect(GameObject effect, Transform body)
        {
            var obj = Instantiate(effect, body);
            var renderer = obj.GetComponent<Renderer>();
            var targetObjectRenderer = body.GetComponentInChildren<SpriteRenderer>();
            renderer.sortingOrder = targetObjectRenderer.sortingOrder;
            renderer.sortingLayerID = targetObjectRenderer.sortingLayerID;
            obj.SetActive(false);
            return obj;
        }
    }
}
