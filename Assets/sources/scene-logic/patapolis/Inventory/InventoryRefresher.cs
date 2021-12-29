namespace PataRoad.SceneLogic.Patapolis
{
    /// <summary>
    /// Just the inventory loader doesn't refresh array by itself...
    /// </summary>
    class InventoryRefresher : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField]
        private Common.Navigator.ActionEventMap _parentSelector;
        [UnityEngine.SerializeField]
        private UnityEngine.GameObject _template;
        [UnityEngine.SerializeField]
        private UnityEngine.Transform _attachTarget;

        private InventoryLoader _currentObject;
        private void Start()
        {
            Append();
        }
        public void Refresh()
        {
            Destroy(_currentObject.gameObject);
            Append();
        }
        private void Append()
        {
            _currentObject = Instantiate(_template, _attachTarget).GetComponent<InventoryLoader>();
            _currentObject.Init(_parentSelector);
        }
        public void Open() => _currentObject.Open();
    }
}
