using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments;
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

        public virtual Items.EquipmentData WeaponData { get; protected set; }
        public virtual Items.EquipmentData ProtectorData { get; protected set; }

        public string WeaponName { get; private set; }
        public string ProtectorName { get; private set; }
        private readonly static Dictionary<ClassType, (string weapon, string protector)> _weaponNameMap = new Dictionary<ClassType, (string, string)>()
        {
            { ClassType.Tatepon, ("Sword","Shield")},
            { ClassType.Dekapon, ("Club","Shoulders")},
            { ClassType.Robopon, ("Arms",null)},
            { ClassType.Kibapon, ("Lance","Horse")},
            { ClassType.Yaripon, ("Spear",null)},
            { ClassType.Megapon, ("Horn","Cape")},
            { ClassType.Toripon, ("Javelin","Bird")},
            { ClassType.Yumipon, ("Bow", null)},
            { ClassType.Mahopon, ("Staff","Shoes")},
        };
        private Dictionary<EquipmentType, string> _nameByEquipmentType = new Dictionary<EquipmentType, string>()
        {
            { EquipmentType.Helm, "Helm" },
            { EquipmentType.Rarepon, "Rarepon" },
        };
        private void Awake()
        {
            (WeaponName, ProtectorName) = _weaponNameMap[Type];
            _nameByEquipmentType.Add(EquipmentType.Weapon, WeaponName);
            _nameByEquipmentType.Add(EquipmentType.Protector, ProtectorName);
        }

        public virtual void Init()
        {
            if (Stat != null) return;
            InitStat();

            SetEquipments();
            InitEquipment(WeaponData, ProtectorData);
        }

        private void InitStat()
        {
            Stat = _defaultStat;
        }
        protected abstract void SetEquipments();

        private void InitEquipment(Items.EquipmentData weaponData, Items.EquipmentData protectorData)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            EquipmentManager = new EquipmentManager(this);
            EquipmentManager.Init();
            Equip(weaponData);
            Equip(protectorData);
        }
        protected void Equip(Items.EquipmentData data)
        {
            if (data != null) EquipmentManager.Equip(data, _defaultStat);
        }
        public void AddMass(float mass) => Rigidbody.mass += mass;
        public string GetEquipmentName(EquipmentType type) => _nameByEquipmentType[type];
    }
}
