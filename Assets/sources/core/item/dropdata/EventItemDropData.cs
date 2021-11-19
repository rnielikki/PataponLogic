using UnityEngine;

namespace PataRoad.Core.Items
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "event-item", menuName = "ItemDrop/Event")]
    public class EventItemDropData : ItemDropData
    {
        [Header("Image Info")]
        [SerializeField]
        protected Sprite _image;
        public Sprite Image => _image;
    }
}
