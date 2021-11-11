using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm...
    /// </summary>
    public class Equipment : MonoBehaviour
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
        protected SpriteRenderer[] _spriteRenderers;

        private EquipmentData _currentData;

        [SerializeField]
        protected EquipmentType _type;
        public EquipmentType Type => _type;

        private void Awake()
        {
            Load();
        }
        internal void Load()
        {
            LoadRenderersAndImage();
            Holder = GetComponentInParent<SmallCharacter>();
        }
        internal virtual void ReplaceEqupiment(EquipmentData equipmentData, Stat stat)
        {
            //Sometimes null but why
            if (Holder == null) Holder = GetComponentInParent<SmallCharacter>();

            if (Type != equipmentData.Type) return;
            if (_currentData != null) Disarm(stat);
            ReplaceImage(equipmentData);

            _stat = equipmentData.Stat;
            _mass = equipmentData.Mass;
            _currentData = equipmentData;
            Arm(stat);
        }
        void Disarm(Stat stat)
        {
            stat.Subtract(_stat);
            Holder.AddMass(-_mass);
        }
        void Arm(Stat stat)
        {
            stat.Add(_stat);
            Holder.AddMass(_mass);
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
                renderer.sprite = equipmentData.Image;
            }
        }
    }
}
