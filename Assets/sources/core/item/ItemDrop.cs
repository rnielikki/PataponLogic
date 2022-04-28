using PataRoad.Core.Global;
using UnityEngine;

namespace PataRoad.Core.Items
{
    public class ItemDrop : MonoBehaviour
    {
        public IItem Item { get; private set; }
        private float _timeToExist;
        private float _fullTimeToExist;
        private SpriteRenderer _backgroundRenderer;
        protected SpriteRenderer _renderer;
        private bool _moving;
        private bool _obtained;
        private bool _doNotDestroy;

        private AudioClip _sound;
        protected UnityEngine.Events.UnityEvent _events;
        private static int _currentDropId { get; set; }
        public int DropId { get; private set; }

        protected void SetItem(ObtainableItemDropData data, IItem item = null)
        {
            Init(data, true);

            Item = item ?? data.Item;
            _renderer.sprite = Item.Image;

            if (Item.ItemType == ItemType.Equipment && Item is EquipmentData eq)
            {
                _renderer.transform.position += (Vector3)eq.GetPivotOffset();
            }
        }
        protected void SetItem(EventItemDropData data)
        {
            Init(data, false);
            _renderer.sprite = data.Image;
        }
        private void Init(ItemDropData data, bool obtainable)
        {
            if (data.Sound == null) _sound = ItemManager.Current.ObtainingSound;
            else _sound = data.Sound;

            _events = data.Events;
            if (obtainable) _events.AddListener(AddToScreen);

            _backgroundRenderer = GetComponent<SpriteRenderer>();
            _renderer = transform.Find("Item").GetComponent<SpriteRenderer>();
            _doNotDestroy = data.DoNotDestroy;

            _fullTimeToExist = _timeToExist = data.TimeToExist;
            DropId = _currentDropId++;
        }
        public static bool DropItem(ItemDropData data, Vector2 position, bool noDestroy = false, IItem replacedItem = null)
        {
            switch (data)
            {
                case ObtainableItemDropData obtainableItemData:
                    var item = replacedItem ?? obtainableItemData.Item;
                    if (item == null || (item.IsUniqueItem && GlobalData.CurrentSlot.Inventory.HasItem(item))
                        || ItemManager.HasUniqueItem(item))
                    {
                        return false;
                    }
                    GetItemDropGameObject(ItemManager.Current.ItemDropTemplate, position).GetComponent<ItemDrop>()
                        .SetItem(obtainableItemData, item);
                    return true;
                case EventItemDropData eventItemData:
                    if (eventItemData.Image == null) return false;
                    GetItemDropGameObject(ItemManager.Current.ItemDropTemplate, position).GetComponent<ItemDrop>().SetItem(eventItemData);
                    return true;
                default:
                    return false;
            }
        }

        protected static GameObject GetItemDropGameObject(GameObject template, Vector2 position)
        {
            var itemInstance = Instantiate(template, ItemManager.Current.ItemDropPoint);
            itemInstance.transform.position = new Vector3(position.x, 0, 0);
            return itemInstance;
        }
        private void Update()
        {
            if (_doNotDestroy) return;
            _timeToExist -= Time.deltaTime;
            var color = new Color(1, 1, 1, _timeToExist / _fullTimeToExist);
            if (_timeToExist <= 0)
            {
                Destroy(gameObject);
            }
            if (!_moving)
            {
                _backgroundRenderer.color = _renderer.color = color;
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Camera.main.ViewportToWorldPoint(Vector2.one),
                    30 * Time.deltaTime);
                _renderer.color = color;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_obtained) return;
            _obtained = true;

            GetComponent<Collider2D>().enabled = false;
            _backgroundRenderer.enabled = false;

            //----------------------
            GameSound.SpeakManager.Current.Play(_sound);
            DoAction(collision);
            //----------------------

            _renderer.color = Color.white;
            _fullTimeToExist = _timeToExist = 0.5f;
            _moving = true;
            _doNotDestroy = false;
        }
        protected virtual void DoAction(Collider2D collision)
        {
            _events.Invoke();
            _events.RemoveAllListeners();
        }
        private void AddToScreen() => ItemManager.AddToScreen(this);
    }
}
