using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    internal abstract class InventoryTabElement : MonoBehaviour
    {
        private Canvas _canvas;
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }
        internal virtual void Select(InventoryLoader inventoryLoader)
        {
            _canvas.sortingOrder = 1;
        }
        internal virtual void Deselect()
        {
            _canvas.sortingOrder = -1;
        }
    }
}
