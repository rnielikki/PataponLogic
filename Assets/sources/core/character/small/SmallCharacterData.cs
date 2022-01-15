using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments;
using PataRoad.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Basic data that can be also used for outside mission.
    /// </summary>
    public abstract class SmallCharacterData : MonoBehaviour
    {
        /// <summary>
        /// Default stat that a bit varies for each class.
        /// </summary>
        [SerializeReference]
        protected Stat _defaultStat = Stat.GetAnyDefaultStatForCharacter();
        [SerializeReference]
        [Tooltip("Attack type resistance when Rarepon isn't used")]
        private AttackTypeResistance _defaultAttackTypeResistance = new AttackTypeResistance();

        public AttackTypeResistance AttackTypeResistance { get; private set; }

        [SerializeField]
        private Equipments.Weapons.ElementalAttackType _elementalAttackType;
        public Equipments.Weapons.ElementalAttackType ElementalAttackType { get; set; }
        /// <summary>
        /// Current Stat.
        /// </summary>
        public virtual Stat Stat { get; protected set; }

        /// <summary>
        /// Class (e.g. Yaripon, Tatepon, Yumipon...) of the Patapon.
        /// </summary>
        [SerializeField]
        protected ClassType _type;
        public ClassType Type => _type;

        internal EquipmentManager EquipmentManager { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }

        public Animator Animator { get; private set; }

        public virtual void Init()
        {
            ElementalAttackType = _elementalAttackType;
            Animator = GetComponent<Animator>();
            if (Stat != null) return;
            InitStat();

            InitEquipment(GetEquipmentData());
            AttackTypeResistance = EquipmentManager.Rarepon?.CurrentData == null ?
            _defaultAttackTypeResistance :
            _defaultAttackTypeResistance.Apply((EquipmentManager.Rarepon.CurrentData as Equipments.Weapons.RareponData).AttackTypeResistance);
        }
        protected abstract IEnumerable<EquipmentData> GetEquipmentData();

        private void InitStat()
        {
            Stat = _defaultStat;
        }

        private void InitEquipment(IEnumerable<EquipmentData> equipmentData)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            EquipmentManager = new EquipmentManager(this);
            EquipmentManager.Init(equipmentData);
        }
        public void Equip(EquipmentData data) => EquipmentManager.Equip(data);
        public void AddMass(float mass) => Rigidbody.mass += mass;
    }
}
