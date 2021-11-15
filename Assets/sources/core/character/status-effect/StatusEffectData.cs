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

        private Dictionary<StatusEffectType, GameObject> _statusEffectMap;

        private void Awake()
        {
            _statusEffectMap = new Dictionary<StatusEffectType, GameObject>()
            {
                { StatusEffectType.Fire, _fireEffect },
                { StatusEffectType.Ice, _iceEffect },
                { StatusEffectType.Sleep, _sleepEffect }
            };
        }
        public GameObject AttachEffect(StatusEffectType type, Transform body)
        {
            var obj = Instantiate(_statusEffectMap[type], body);
            var renderer = obj.GetComponent<Renderer>();
            var targetObjectRenderer = body.GetComponentInChildren<SpriteRenderer>();
            renderer.sortingOrder = targetObjectRenderer.sortingOrder;
            renderer.sortingLayerID = targetObjectRenderer.sortingLayerID;
            return obj;
        }
    }
}
