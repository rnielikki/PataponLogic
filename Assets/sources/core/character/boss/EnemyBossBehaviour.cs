using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Character.Bosses
{
    internal abstract class EnemyBossBehaviour : UnityEngine.MonoBehaviour
    {
        protected EnemyBoss _boss;
        protected Patapons.PataponsManager _pataponsManager;
        protected BossTurnManager _turnManager { get; private set; }
        protected int _level => _boss.Level;

        public bool PartBroken { get; private set; }

        protected readonly List<string> _attackCombos =
            new List<string>();

        protected virtual string[][] _predefinedCombos { get; set; } = new string[][] { };
        private readonly Dictionary<int, List<string[]>> _predefinedCombosIndexed = new Dictionary<int, List<string[]>>();
        private int[] _predefinedComboLengthIndexed;

        internal void Init(EnemyBoss boss, Patapons.PataponsManager pataponsManager)
        {
            if (_boss != null || boss == null) return;
            _boss = boss;
            _pataponsManager = pataponsManager;
            _turnManager = _boss.BossTurnManager;
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
            Init();
        }
        protected virtual void Init() { }
        public virtual float CalculateAttack()
        {
            //from level3 it will do combo attk
            var comboCount = UnityEngine.Random.Range(1,
                UnityEngine.Mathf.RoundToInt(UnityEngine.Mathf.Sqrt(_level)) + 1);

            return SetComboAttack(comboCount);
        }
        public virtual void CalculateAttackOnIce()
        {
            _turnManager.SetOneAction(GetNextBehaviourOnIce());
        }
        public void MarkAsPartBroken()
        {
            PartBroken = true;
        }

        protected float SetComboAttack(int count)
        {
            if (count <= 0)
            {
                _turnManager.SetOneAction("nothing");
                return 5;
            }
            else if (count == 1)
            {
                var next = GetNextBehaviour();
                _turnManager.SetOneAction(next.action);
                return next.distance;
            }
            else
            {
                _attackCombos.Clear();
                var distance = SetMultipleAttacks(count);
                _turnManager.SetComboAttack(_attackCombos);
                return distance;
            }
        }
        protected abstract (string action, float distance) GetNextBehaviour();
        protected abstract string GetNextBehaviourOnIce();
        protected virtual float SetMultipleAttacks(int count)
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
                    int selectedAttackIndex = UnityEngine.Random.Range(0, allChoices.Count - 1);
                    foreach (var attack in allChoices[selectedAttackIndex])
                    {
                        _attackCombos.Add(attack);
                    }
                }
                leftComboCount = count - _attackCombos.Count;
            }
            return 1;

        }
        protected virtual float SetMultipleAttacksOneByOne(int count)
        {
            float distance = UnityEngine.Mathf.Infinity;
            for (int i = 0; i < count; i++)
            {
                var next = GetNextBehaviour();
                _attackCombos.Add(next.action);
                distance = UnityEngine.Mathf.Min(next.distance, distance);
            }
            return distance;
        }
    }
}
