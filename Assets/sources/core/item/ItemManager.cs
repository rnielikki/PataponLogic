using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Items
{
    internal class ItemManager : MonoBehaviour
    {
        public static ItemManager Current { get; private set; }
        readonly Dictionary<System.Guid, ItemStatusUpdater> _itemMap = new Dictionary<System.Guid, ItemStatusUpdater>();
        private RectTransform _rectTransform;

        [SerializeField]
        private AudioClip _obtainingSound;
        public AudioClip ObtainingSound => _obtainingSound;

        [SerializeField]
        private GameObject _itemDropTemplate;
        public GameObject ItemDropTemplate => _itemDropTemplate;

        [SerializeField]
        private GameObject _itemInScreenTemplate;

        private void Awake()
        {
            Current = this;
            _rectTransform = GetComponent<RectTransform>();
        }
        private void OnDestroy()
        {
            Current = null;
        }

        internal static void AddToScreen(IItem item)
        {
            if (!Current._itemMap.TryGetValue(item.Id, out ItemStatusUpdater updater))
            {
                var obj = Instantiate(Current._itemInScreenTemplate, Current.transform);
                var itemStatusUpdater = obj.GetComponent<ItemStatusUpdater>();
                itemStatusUpdater.SetItem(item);
                Current._itemMap.Add(item.Id, itemStatusUpdater);
            }
            else
            {
                //increase count
                updater.IncreaseCount();
            }
        }
    }
}
