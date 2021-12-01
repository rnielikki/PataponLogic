using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Staff : Weapon
    {
        public override float MinAttackDistance { get; } = 22;
        public override float WindAttackDistanceOffset { get; } = 4;

        private IStaffActions _staffAction;
        private void Start()
        {
            Init();
            if (_staffAction == null)
            {
                _staffAction = GetComponentInChildren<IStaffActions>();
            }
            _staffAction.Initialize(Holder);
        }
        public override void Attack(AttackCommandType attackCommandType)
        {
            switch (attackCommandType)
            {
                case AttackCommandType.Attack:
                    _staffAction.NormalAttack();
                    break;
                case AttackCommandType.ChargeAttack:
                    _staffAction.ChargeAttack();
                    break;
                case AttackCommandType.Defend:
                    _staffAction.Defend();
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
            _staffAction = null;

            foreach (var gameObj in (equipmentData as StaffData).AdditionalPrefabs)
            {
                var ins = Instantiate(gameObj, transform);
                var staffData = ins.GetComponent<IStaffActions>();
                //GetComponentInChldren<> doesn't work in some reason...
                if (staffData != null) _staffAction = staffData;
            }
        }
    }
}
