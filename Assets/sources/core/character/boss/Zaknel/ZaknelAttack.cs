using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    class ZaknelAttack : BossAttackData
    {
        [SerializeField]
        private BossAttackParticle Fire;
        private readonly System.Collections.Generic.List<GameObject> _raycastColliders
            = new System.Collections.Generic.List<GameObject>();
        private LayerMask _noRayLayer;
        private LayerMask _rayLayer;

        protected override void Init()
        {
            CharacterSize = 5;
            base.Init();
        }
        private void Start()
        {
            var layers = CharacterTypeDataCollection.GetCharacterDataByType(Boss);
            _rayLayer = layers.SelfLayerMaskRayCast;
            _noRayLayer = layers.SelfLayerMaskNoRayCast;
            _raycastColliders.AddRange(from cl in GetComponentsInChildren<Collider2D>(true)
                                       where cl.gameObject.layer == _rayLayer
                                       select cl.gameObject);
        }
        public void FireAttack()
        {
            Fire.Attack();
        }
        public void EarthquakeAttack()
        {
            Boss.AttackType = Equipments.Weapons.AttackType.Crush;
            Boss.ElementalAttackType = Equipments.Weapons.ElementalAttackType.Neutral;
            Boss.StatusEffectManager.TumbleAttack(true);
        }
        //preventing pushback
        public void StartWheelAttackMode()
        {
            foreach (var ray in _raycastColliders)
            {
                ray.layer = _noRayLayer;
            }
        }
        public void ToDefaultPosition()
        {
            var pos = transform.position;
            pos.x = Boss.DefaultWorldPosition;
            transform.position = pos;
            foreach (var ray in _raycastColliders)
            {
                ray.layer = _rayLayer;
            }
        }
        internal override void UpdateStatForBoss(int level)
        {
            var value = 0.8f + (level * 0.2f);
            _stat.MultipleDamage(value);
            _stat.DefenceMin += (level - 1) * 0.005f;
            _stat.DefenceMax += (level - 1) * 0.01f;
            Boss.SetMaximumHitPoint(Mathf.RoundToInt(_stat.HitPoint * value));

            Stat.AddCriticalResistance(level * 0.05f);
            Stat.AddStaggerResistance(level * 0.05f);

            Stat.AddFireResistance(level * 0.03f);
            Stat.AddIceResistance(level * 0.03f);
            Stat.AddSleepResistance(level * 0.035f);
        }
    }
}
