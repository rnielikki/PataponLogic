using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm...
    /// </summary>
    public abstract class Equipment : MonoBehaviour
    {
        /// <summary>
        /// The stat bonus that attached to the weapon.
        /// </summary>
        [SerializeReference]
        protected Stat _stat = new Stat();
        public Stat Stat => _stat;
        /// <summary>
        /// Mass of the weapon. The mass affects to the knockback distance. Also it affects to the wind effect.
        /// </summary>
        /// <note>This won't affect to the throwing distance.</note>
        [SerializeField]
        private float _mass;
        public float Mass => _mass;

        public SmallCharacter Holder { get; protected set; }
        public SmallCharacterData HolderData { get; protected set; }
        protected SpriteRenderer[] _spriteRenderers;

        protected EquipmentData _currentData;
        protected abstract EquipmentType _type { get; }

        [SerializeField]
        [Tooltip("Never replace this equipment")]
        protected bool _fixedEquipment;
        public bool FixedEquipment => _fixedEquipment;

        [SerializeField]
        [Tooltip("Can equip, but not be shown. For general.")]
        protected bool _hideEquipment;
        private void Awake()
        {
            LoadRenderersAndImage();
        }

        private void Start()
        {
            Load();
            if (!_fixedEquipment) _currentData = ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, HolderData.GetEquipmentName(_type), 0);
        }
        internal void Load()
        {
            Holder = GetComponentInParent<SmallCharacter>();
            HolderData = GetComponentInParent<SmallCharacterData>();
        }
        internal virtual void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            //Sometimes null but why
            if (Holder == null) Holder = GetComponentInParent<SmallCharacter>();
            if (HolderData == null) HolderData = GetComponentInParent<SmallCharacterData>();

            RemoveDataFromStat(stat);
            if (!_hideEquipment) ReplaceImage(equipmentData);

            _stat = equipmentData.Stat;
            _mass = equipmentData.Mass;
            _currentData = equipmentData;

            AddDataToStat(stat);
        }
        protected void RemoveDataFromStat(Stat stat)
        {
            stat.Subtract(_stat);
            HolderData.AddMass(-_mass);
        }
        protected void AddDataToStat(Stat stat)
        {
            stat.Add(_stat);
            HolderData.AddMass(_mass);
        }
        protected virtual void LoadRenderersAndImage()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(false);
        }

        protected virtual void ReplaceImage(EquipmentData equipmentData)
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            foreach (var renderer in _spriteRenderers)
            {
                renderer.sprite = equipmentData?.Image;
            }
        }
    }
}
