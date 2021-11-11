using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    /// <summary>
    /// Abstraction of some kind of staff behaviours. Add Monobehaviour that uses <see cref="IStaffData"/> to CHILD.
    /// </summary>
    public class Staff : Weapon
    {
        public override float MinAttackDistance { get; } = 22;
        public override float WindAttackDistanceOffset { get; } = 4;
        private IStaffData _staffData;
        private void Start()
        {
            Init();
            if (_staffData == null)
            {
                _staffData = GetComponentInChildren<IStaffData>();
            }
            _staffData.Initialize(Holder);
        }
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    _staffData.NormalAttack();
                    break;
                case AttackCommandType.ChargeAttack:
                    _staffData.ChargeAttack();
                    break;
                case AttackCommandType.Defend:
                    _staffData.Defend();
                    break;
            }
        }

        //be careful, don't change but just copy from equipmentData!
        internal override void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            base.ReplaceEqupiment(equipmentData, stat);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            _staffData = null;

            foreach (Transform child in equipmentData.transform)
            {
                var ins = Instantiate(child.gameObject, transform);
                var staffData = ins.GetComponent<IStaffData>();
                //GetComponentInChldren<> doesn't work in some reason...
                if (staffData != null) _staffData = staffData;
            }
        }
    }
}
