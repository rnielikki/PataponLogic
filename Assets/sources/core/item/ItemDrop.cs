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
            _sound = sound ?? ItemManager.Current.ObtainingSound;
            _action = action ?? AddToScreen;
            _backgroundRenderer = GetComponent<SpriteRenderer>();
            _renderer = transform.Find("Item").GetComponent<SpriteRenderer>();

            _timeToExist = time;
            _fullTimeToExist = time;
        }

        public static void DropItemOnRandom(Sprite image, Vector2 position, float timeToExist, float chance, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            DropItem(image, position, timeToExist, action, sound);
        }
        public static void DropItemOnRandom(IItem item, Vector2 position, float timeToExist, float chance, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            if (Random.Range(0, 1) < Mathf.Clamp01(chance)) DropItem(item, position, timeToExist, action, sound);
        }
        public static void DropItem(IItem item, Vector2 position, float timeToExist, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            GetItemDropGameObject(position).GetComponent<ItemDrop>().SetItem(item, timeToExist, action, sound);
        }
        public static void DropItem(Sprite image, Vector2 position, float timeToExist, UnityEngine.Events.UnityAction action = null, AudioClip sound = null)
        {
            GetItemDropGameObject(position).GetComponent<ItemDrop>().SetItem(image, timeToExist, action, sound);
        }
        private static GameObject GetItemDropGameObject(Vector2 position)
        {
            var itemInstance = Instantiate(ItemManager.Current.ItemDropTemplate);
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
