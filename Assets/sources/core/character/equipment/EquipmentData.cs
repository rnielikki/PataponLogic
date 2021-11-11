using UnityEngine;

namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
    /// </summary>
    public class EquipmentData : MonoBehaviour, Items.IItem
    {
        ///<summary>
        /// Name of the equipment, e.g. "Wooden Shield", "Divine Sword"
        /// </summary>
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeField]
        private string _description;
        public string Description => _description;
        /// <summary>
        /// The stat bonus that attached to the weapon.
        /// </summary>
        [SerializeReference]
        protected Stat _stat = new Stat();
        public Stat Stat => _stat;

        /// <summary>
        /// Sprite image of the weapon.
        /// </summary>
        public Sprite Image { get; protected set; }

        /// <summary>
        /// Mass of the weapon. The mass affects to the knockback distance. Also it affects to the wind effect.
        /// </summary>
        /// <note>This won't affect to the throwing distance.</note>
        [SerializeField]
        private float _mass;
        public float Mass => _mass;


        [SerializeField]
        protected EquipmentType _type;
        public EquipmentType Type => _type;

        public void LoadImage()
        {
            if (Image != null) return;
            Image = GetComponent<SpriteRenderer>().sprite;
        }
        public void LoadImage(string transformName)
        {
            if (Image != null) return;
            Image = transform.Find(transformName).GetComponent<SpriteRenderer>().sprite;
        }
        public Sprite FindSprite(string transformName) => transform.Find(transformName).GetComponent<SpriteRenderer>().sprite;
    }
}
