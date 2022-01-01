using System;
using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class SummonedBoss : Boss
    {
        [SerializeField]
        [Tooltip("This will read automatically level from map and will update boss level")]
        int _connectedMapIndex = -1;
        public override Vector2 MovingDirection { get; } = Vector2.right;
        private Transform _pataponManagerTransform;
        public override float DefaultWorldPosition => _pataponManagerTransform.position.x;
        private BossSummonManager _manager;
        private bool _animatingWalking;
        private bool _animatingIdle = true;
        private bool _attacking;

        private float _lastPerfectionRate;
        private bool _charged;
        private SpriteRenderer[] _renderers;
        protected float _offsetFromManager;

        protected override void Init(BossAttackData data)
        {
            base.Init(data);
            var map = Global.GlobalData.MapInfo.GetMapByIndex(_connectedMapIndex);
            BossAttackData.UpdateStatForBoss(map.Level);
            _pataponManagerTransform = FindObjectOfType<Patapons.PataponsManager>().transform;
            DistanceCalculator = DistanceCalculator.GetBossDistanceCalculator(this);
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }
        internal void SetManager(BossSummonManager bossSummonManager) => _manager = bossSummonManager;
        public override float GetAttackValueOffset() => _lastPerfectionRate;
        public override float GetDefenceValueOffset() => _lastPerfectionRate;

        protected abstract void Ponpon();
        protected abstract void ChargedPonpon();
        protected abstract void Chakachaka();
        protected abstract void ChargedChakachaka();
        public void Act(Rhythm.Command.RhythmCommandModel model)
        {
            var song = model.Song;
            _lastPerfectionRate = model.AccuracyRate;

            if (StatusEffectManager.CanContinue)
            {
                _attacking = PerformCommandAction(song);
                if (_attacking)
                {
                    _animatingWalking = false;
                    _animatingIdle = false;
                }
            }
            //Should be executed AFTER command execusion, for avoiding command bug
            _charged = song == Rhythm.Command.CommandSong.Ponchaka; //Removes charged status if it isn't charging command
        }

        //returns if attacking command
        private bool PerformCommandAction(Rhythm.Command.CommandSong song)
        {
            switch (song)
            {
                case Rhythm.Command.CommandSong.Ponpon:
                    if (_charged) ChargedPonpon();
                    else Ponpon();
                    return true;
                case Rhythm.Command.CommandSong.Chakachaka:
                    if (_charged) ChargedChakachaka();
                    else Chakachaka();
                    return true;
                //boss won't be really "protected" but "cured" - boss should have high resistance for everything!
                case Rhythm.Command.CommandSong.Donchaka:
                    StatusEffectManager.Recover();
                    break;
            }
            _animatingWalking = false;
            _animatingIdle = true;
            CharAnimator.Animate("Idle");

            return false;
        }
        public override void Die()
        {
            _manager.MarkAsDead();
            base.Die();
            transform.parent = transform.root.parent;
        }

        private void Update()
        {
            if ((Rhythm.Command.TurnCounter.IsPlayerTurn || !_attacking) && StatusEffectManager.CanContinue)
            {
                var targetPosition = _manager.transform.position + _offsetFromManager * Vector3.left;
                var offset = Stat.MovementSpeed * Time.deltaTime;
                if (!DistanceCalculator.IsInTargetRange(targetPosition.x, offset))
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, offset);
                    if (!_animatingWalking)
                    {
                        CharAnimator.Animate("walk");
                        _animatingWalking = true;
                        _animatingIdle = false;
                    }
                }
                else
                {
                    if (!_animatingIdle) CharAnimator.Animate("Idle");
                    _animatingWalking = false;
                    _animatingIdle = true;
                }
            }
            else if (IsDead)
            {
                foreach (var renderer in _renderers)
                {
                    var color = renderer.color;
                    var a = color.a - 0.2f * Time.deltaTime;
                    if (a > 0)
                    {
                        color.a = a;
                        renderer.color = color;
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
        private void OnValidate()
        {
            if (_connectedMapIndex < 0) throw new MissingFieldException("Set proper map index to reference level!");
        }
    }
}
