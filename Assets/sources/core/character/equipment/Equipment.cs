using PataRoad.Core.Items;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm...
    /// </summary>
    public abstract class Equipment : MonoBehaviour
    {
        public Stat Stat => CurrentData.Stat;
        public float Mass => CurrentData.Mass;

        public SmallCharacter Holder { get; protected set; }
        public SmallCharacterData HolderData { get; protected set; }
        protected SpriteRenderer[] _spriteRenderers;

        public EquipmentData CurrentData { get; protected set; }
        protected abstract EquipmentType _type { get; }

        [SerializeField]
        [Tooltip("Never replace this equipment")]
        protected bool _fixedEquipment;
        public bool FixedEquipment => _fixedEquipment;

        [SerializeField]
        [Tooltip("Can equip, but not be shown. For general.")]
        protected bool _hideEquipment;
        [SerializeField]
        protected EquipmentData _defaultEquipmentData;

        private void Awake()
        {
            LoadRenderersAndImage();
        }
        private void Start()
        {
            Load();
        }
        internal void Load()
        {
            if (_spriteRenderers == null) LoadRenderersAndImage();
            if (HolderData != null && _defaultEquipmentData != null) return;
            if (Holder == null) Holder = GetComponentInParent<SmallCharacter>();
            HolderData = GetComponentInParent<SmallCharacterData>();
            if (!_fixedEquipment)
            {
                _defaultEquipmentData = GetDefault();
            }
            CurrentData = _defaultEquipmentData;
        }
        protected virtual EquipmentData GetDefault() => ItemLoader.GetItem<EquipmentData>(ItemType.Equipment, Class.ClassAttackEquipmentData.GetEquipmentName(HolderData.Type, _type), 0);
        internal virtual void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            //Sometimes null but why
            if (Holder == null) Holder = GetComponentInParent<SmallCharacter>();
            if (HolderData == null) Load();

            RemoveDataFromStat(stat);

            if (!_hideEquipment) ReplaceImage(equipmentData);

            CurrentData = equipmentData;

            AddDataToStat(stat);
        }
        internal void EquipDefault(Stat stat)
        {
            if (HolderData == null) Load();
            ReplaceEqupiment(_defaultEquipmentData, stat);
        }
        protected void RemoveDataFromStat(Stat stat)
        {
            if (CurrentData == null) return;
            stat.Subtract(Stat);
            HolderData.AddMass(-Mass);
        }
        protected virtual void AddDataToStat(Stat stat)
        {
            stat.Add(Stat);
            HolderData.AddMass(Mass);
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
        private void OnValidate()
        {
            if (_fixedEquipment && _defaultEquipmentData == null)
            {
                Debug.LogError($"Please set equipment data for {name} or disable fixed equipment.");
            }
        }
    }
}
