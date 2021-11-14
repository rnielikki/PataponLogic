using UnityEngine;

namespace PataRoad.Core.Items
{
    public class ItemDrop : MonoBehaviour
    {
        private IItem _item;
        private float _timeToExist;
        private float _fullTimeToExist;
        private SpriteRenderer _backgroundRenderer;
        private SpriteRenderer _renderer;
        private bool _moving;
        private bool _obtained;

        private AudioClip _sound;
        private UnityEngine.Events.UnityAction _action;

        // Start is called before the first frame update
        private void SetItem(IItem item, float time, UnityEngine.Events.UnityAction action, AudioClip sound)
        {
            Init(time, action, sound);

            _item = item;
            _renderer.sprite = item.Image;

            if (item is EquipmentData eq)
            {
                _renderer.transform.position += (Vector3)eq.GetPivotOffset();
            }
        }
        private void SetItem(Sprite image, float time, UnityEngine.Events.UnityAction action, AudioClip sound)
        {
            Init(time, action, sound);
            _renderer.sprite = image;
        }
        private void Init(float time, UnityEngine.Events.UnityAction action, AudioClip sound)
        {
            //Should do same thing but simply doesn't work. Weird.
            //_sound = sound ?? ItemManager.Current.ObtainingSound
            if (sound == null) _sound = ItemManager.Current.ObtainingSound;
            else _sound = sound;
            _action = action ?? AddToScreen;
            _backgroundRenderer = GetComponent<SpriteRenderer>();
            _renderer = transform.Find("Item").GetComponent<SpriteRenderer>();

            _timeToExist = time;
            _fullTimeToExist = time;
        }

        /// <summary>
        /// Drops item by item data.
        /// </summary>
        /// <param name="item">Item, from <see cref="ItemLoader.GetItem(ItemType, string, int)"/> or <see cref="ItemLoader.GetRandomItem(ItemType, int, int)"/>.</param>
        /// <param name="position">position to drop.</param>
        /// <param name="timeToExist">After this time, item will be disappear.</param>
        /// <param name="action">Action to perform when get item. If <c>null</c>, player "obtains" the item.</param>
        /// <param name="sound">Sound to play when get item. If <c>null</c>, default Patapon item obtaining sound will be played.</param>
        /// <returns><c>true</c> if item is successfully dropped, otherwise <c>false</c>.</returns>
        /// <note>Hint: The return value can be used for unique item drop sequence.</note>
        public static bool DropItem(IItem item, Vector2 position, float timeToExist, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            if (item == null) return false;
            GetItemDropGameObject(position).GetComponent<ItemDrop>().SetItem(item, timeToExist, action, sound);
            return true;
        }

        /// <summary>
        /// Drops item by image and action.
        /// </summary>
        /// <param name="image">The image to show on item.</param>
        /// <param name="position">Item position to drop.</param>
        /// <param name="timeToExist">After this time, item will be disappear.</param>
        /// <param name="action">Action to perform when get item. If <c>null</c>, the item drop ALWAYS FAILS.</param>
        /// <param name="sound">Sound to play when get item. If <c>null</c>, default Patapon item obtaining sound will be played.</param>
        /// <returns><c>true</c> if item is successfully dropped, otherwise <c>false</c>.</returns>
        public static bool DropItem(Sprite image, Vector2 position, float timeToExist, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            if (image == null || action == null) return false;
            GetItemDropGameObject(position).GetComponent<ItemDrop>().SetItem(image, timeToExist, action, sound);
            return true;
        }
        private static GameObject GetItemDropGameObject(Vector2 position)
        {
            var itemInstance = Instantiate(ItemManager.Current.ItemDropTemplate, ItemManager.Current.ItemDropPoint);
            itemInstance.transform.position = position;
            return itemInstance;
        }
        private void Update()
        {
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
                transform.position = Vector2.MoveTowards(transform.position, Camera.main.ViewportToWorldPoint(Vector2.one), 30 * Time.deltaTime);
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
            _action();
            //----------------------

            _renderer.color = Color.white;
            _fullTimeToExist = _timeToExist = 0.5f;
            _moving = true;
        }
        private void AddToScreen() => ItemManager.AddToScreen(_item);
    }
}
