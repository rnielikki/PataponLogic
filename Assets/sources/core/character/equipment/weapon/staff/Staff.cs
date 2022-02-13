using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    public class Staff : Weapon
    {
        private IStaffActions _staffAction;
        protected override float _throwMass => 0.1f;
        public override bool IsTargetingCenter => true;

        [SerializeField]
        Color _fireColor;
        [SerializeField]
        Color _iceColor;
        [SerializeField]
        Color _thunderColor;

        private void Start()
        {
            Init();
            if (_staffAction == null)
            {
                _staffAction = GetComponentInChildren<IStaffActions>();
            }
            if (Holder != null)
            {
                _staffAction.Initialize(Holder);
                _staffAction.SetElementalColor(LoadElementalColor(Holder.ElementalAttackType));
            }
            (_staffAction as MonoBehaviour).gameObject.layer
                = CharacterTypeDataCollection.GetCharacterDataByType(Holder).AttackLayer;
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

            var ins = Instantiate((equipmentData as StaffData).AdditionalPrefab, transform);
            var staffData = ins.GetComponent<IStaffActions>();
            //GetComponentInChldren<> doesn't work in some reason...
            if (staffData != null) _staffAction = staffData;

            if (Holder != null) _staffAction?.Initialize(Holder);
        }
        private Color LoadElementalColor(ElementalAttackType elementalAttack) => elementalAttack switch
        {
            ElementalAttackType.Fire => _fireColor,
            ElementalAttackType.Ice => _iceColor,
            ElementalAttackType.Thunder => _thunderColor,
            _ => Color.white
        };
        public override float GetAttackDistance() => 22;
    }
}
