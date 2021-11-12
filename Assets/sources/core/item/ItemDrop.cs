using UnityEngine;

namespace PataRoad.Core.Items
{
    public class ItemDrop : MonoBehaviour
    {
        private IItem _item;
        private static GameObject _template;
        private float _timeToExist;
        private float _fullTimeToExist;
        private SpriteRenderer _renderer;
        // Start is called before the first frame update
        public void SetItem(IItem item, float time)
        {
            _item = item;
            _renderer = transform.Find("Item").GetComponent<SpriteRenderer>();
            _renderer.sprite = item.Image;

            if (item is Character.Equipments.EquipmentData eq)
            {
                _renderer.transform.position += (Vector3)eq.GetPivotOffset();
            }

            _timeToExist = time;
            _fullTimeToExist = time;
        }

        public static void DropItem(IItem item, Vector2 position, float timeToExist)
        {
            if (_template == null) _template = Resources.Load<GameObject>("Map/Items/ItemDrop");
            var itemInstance = Instantiate(_template);
            itemInstance.transform.position = position;
            itemInstance.GetComponent<ItemDrop>().SetItem(item, timeToExist);
        }
        private void Update()
        {
            _timeToExist -= Time.deltaTime;
            if (_timeToExist <= 0) Destroy(gameObject);
            else _renderer.color = new Color(1, 1, 1, _timeToExist / _fullTimeToExist);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);
        }
    }
}
