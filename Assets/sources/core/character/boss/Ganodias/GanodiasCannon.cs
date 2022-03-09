namespace PataRoad.Core.Character.Bosses
{
    class GanodiasCannon : BossAttackComponent
    {
        private DistanceCalculator _calculator;
        private void Start()
        {
            _calculator = GetComponentInParent<Boss>().DistanceCalculator;
        }
        public void Attack()
        {
            var groundedTargets = _calculator.GetAllGroundedTargets();
            foreach (var target in groundedTargets)
            {
                var obj = (target as UnityEngine.MonoBehaviour).gameObject;
                _boss.Attack(this, obj, obj.transform.position,
                    Equipments.Weapons.AttackType.Neutral, Equipments.Weapons.ElementalAttackType.Neutral);
            }
        }
    }
}
