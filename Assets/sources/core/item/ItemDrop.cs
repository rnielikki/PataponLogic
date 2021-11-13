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

        // Start is called before the first frame update
        private void SetItem(IItem item, float time)
        {
            _item = item;
            _backgroundRenderer = GetComponent<SpriteRenderer>();
            _renderer = transform.Find("Item").GetComponent<SpriteRenderer>();
            _renderer.sprite = item.Image;

            if (item is Character.Equipments.EquipmentData eq)
            {
                _renderer.transform.position += (Vector3)eq.GetPivotOffset();
            }

            _timeToExist = time;
            _fullTimeToExist = time;
        }

        public static void DropItemOnRandom(ItemMetadata data, Vector2 position, float timeToExist, float chance)
        {
            DropItem(data, position, timeToExist);
        }
        public static void DropItem(ItemMetadata data, Vector2 position, float timeToExist)
        {
            IItem item = ItemLoader.Load(data);
            if (item == null)
            {
                throw new System.ArgumentException($"No item matches for {data.Type}/{data.Group}/{data.Index}");
            }
            var itemInstance = Instantiate(ItemManager.Current.ItemDropTemplate);
            itemInstance.transform.position = position;
            itemInstance.GetComponent<ItemDrop>().SetItem(item, timeToExist);
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
            GameSound.SpeakManager.Current.Play(ItemManager.Current.ObtainingSound);

            GetComponent<Collider2D>().enabled = false;
            _obtained = true;

            _backgroundRenderer.enabled = false;
            ItemManager.AddToScreen(_item);

            _renderer.color = Color.white;
            _fullTimeToExist = _timeToExist = 0.5f;
            _moving = true;
        }
    }
}
