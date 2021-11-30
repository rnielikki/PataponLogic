using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Equipments;
using UnityEngine;

namespace PataRoad.Core.Character
{
    /// <summary>
    /// Basic data that can be also used for outside mission.
    /// </summary>
    public class SmallCharacterData : MonoBehaviour
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

        [SerializeField]
        Items.EquipmentData _weaponData;
        [SerializeField]
        Items.EquipmentData _protectorData;

        public void Init()
        {
            if (Stat != null) return;
            InitStat();
            InitEquipment(_weaponData, _protectorData);
        }

        private void InitStat()
        {
            Stat = _defaultStat;
        }


        private void InitEquipment(Items.EquipmentData weaponData, Items.EquipmentData protectorData)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            EquipmentManager = new EquipmentManager(gameObject);
            if (weaponData != null) EquipmentManager.Equip(weaponData, _defaultStat);
            if (protectorData != null) EquipmentManager.Equip(protectorData, _defaultStat);
        }
        private void OnValidate()
        {
            if (_weaponData != null && _weaponData.Type != EquipmentType.Weapon)
            {
                throw new System.ArgumentException("Weapon data should be type of weapon but it's " + _weaponData.Type);
            }
            if (_protectorData != null && _protectorData.Type != EquipmentType.Protector)
            {
                throw new System.ArgumentException("Protector should be type of protector but it's " + _protectorData.Type);
            }
        }
    }
}
