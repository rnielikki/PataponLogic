using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Character.Bosses
{
    public abstract class EnemyBossBehaviour : UnityEngine.MonoBehaviour
    {
        public EnemyBoss Boss { get; protected set; }
        protected Patapons.PataponsManager _pataponsManager;
        protected BossTurnManager _turnManager { get; private set; }
        protected int _level => Boss.Level;

        public bool PartBroken { get; private set; }
        protected int _minCombo = 1;

        protected readonly List<string> _attackCombos =
            new List<string>();

        protected virtual string[][] _predefinedCombos { get; set; } = new string[][] { };
        private readonly Dictionary<int, List<string[]>> _predefinedCombosIndexed = new Dictionary<int, List<string[]>>();
        private int[] _predefinedComboLengthIndexed;
        private int _maxCombo = -1;

        public bool UseCustomDataPosition { get; protected set; }
        private bool _lastAttackWasOnIce;

        internal void Init(EnemyBoss boss, Patapons.PataponsManager pataponsManager)
        {
            if (Boss != null || boss == null) return;
            Boss = boss;
            _pataponsManager = pataponsManager;
            _turnManager = Boss.BossTurnManager;
            foreach (var combo in _predefinedCombos)
            {
                var length = combo.Length;
                if (!_predefinedCombosIndexed.ContainsKey(length))
                {
                    _predefinedCombosIndexed.Add(length, new List<string[]>());
                }
                _predefinedCombosIndexed[length].Add(combo);
            }
            _predefinedComboLengthIndexed = _predefinedCombosIndexed.Keys.OrderBy(index => index).ToArray();
            Boss.StatusEffectManager.OnStatusEffect.AddListener(OnStatusEffect);
            Init();
        }
        protected virtual void Init() { }
        //from level9 it will do combo attk (if mincombo is less than 2)
        public virtual (float distance, float maxDistance) CalculateAttack()
        {
            if (_maxCombo < 0)
            {
                _maxCombo = UnityEngine.Mathf.Max(
                    (int)UnityEngine.Mathf.Sqrt(_level) - 1,
                    _minCombo) + 1;
            }
            var comboCount = UnityEngine.Random.Range(1, _maxCombo);

            return SetComboAttack(comboCount);
        }
        public virtual void CalculateAttackOnIce()
        {
            _turnManager.SetOneAction(GetNextBehaviourOnIce());
            _lastAttackWasOnIce = true;
        }
        public void MarkAsPartBroken()
        {
            PartBroken = true;
        }

        protected (float distance, float maxDistance) SetComboAttack(int count)
        {
            _lastAttackWasOnIce = false;
            if (count <= 0)
            {
                _turnManager.SetOneAction("nothing");
                return (5, -1);
            }
            else if (count == 1)
            {
                var next = GetNextBehaviour();
                _turnManager.SetOneAction(next.Action);
                return (next.Distance, next.MaxDistance);
            }
            else
            {
                _attackCombos.Clear();
                var distance = SetMultipleAttacks(count);
                _turnManager.SetComboAttack(_attackCombos);
                return distance;
            }
        }
        protected abstract BossAttackMoveSegment GetNextBehaviour();
        protected virtual BossAttackMoveSegment GetNextBehaviourForCombo(int index, int comboCount) => GetNextBehaviour();
        protected abstract string GetNextBehaviourOnIce();
        protected virtual (float distance, float maxDistance) SetMultipleAttacks(int count)
        {
            var leftComboCount = count;
            while (leftComboCount > 0)
            {
                if (_predefinedComboLengthIndexed.Length < 1 || leftComboCount < _predefinedComboLengthIndexed[0])
                {
                    return SetMultipleAttacksOneByOne(count);
                }
                else
                {
                    //gets max available predefined attk
                    int i = _predefinedComboLengthIndexed.Length - 1;
                    while (i >= 0 && _predefinedComboLengthIndexed[i] > leftComboCount)
                    {
                        i--;
                    }
                    var allChoices = _predefinedCombosIndexed[_predefinedComboLengthIndexed[i]];
                    //it's MAX EXCLUSIVE
                    int selectedAttackIndex = UnityEngine.Random.Range(0, allChoices.Count);
                    foreach (var attack in allChoices[selectedAttackIndex])
                    {
                        _attackCombos.Add(attack);
                    }
                }
                leftComboCount = count - _attackCombos.Count;
            }
            return (1, -1);
        }
        protected virtual (float distance, float maxDistance) SetMultipleAttacksOneByOne(int count)
        {
            float distance = UnityEngine.Mathf.Infinity;
            float maxDistance = -1;
            for (int i = 0; i < count; i++)
            {
                var next = GetNextBehaviourForCombo(i, count);
                _attackCombos.Add(next.Action);
                distance = UnityEngine.Mathf.Min(next.Distance, distance);
                if (next.MaxDistance >= 0)
                {
                    maxDistance = UnityEngine.Mathf.Max(maxDistance, next.MaxDistance);
                }
            }
            return (distance, maxDistance);
        }
        protected virtual void OnStatusEffect(StatusEffectType type)
        {
            if (type != StatusEffectType.Fire && type != StatusEffectType.Tumble
                && !(type == StatusEffectType.Ice && _lastAttackWasOnIce))
            {
                Boss.BossTurnManager.End();
            }
        }
    }
}
