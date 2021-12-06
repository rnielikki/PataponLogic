using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    class ClassSelectionUpdater : MonoBehaviour
    {
        [SerializeField]
        Text _className;
        [SerializeField]
        Text _description;
        [SerializeField]
        Text _generalName;
        [SerializeField]
        Text _selfEffect;
        [SerializeField]
        Text _groupEffect;
        [SerializeField]
        private RectTransform _statScreen;
        private ClassSelectionInfo _current;

        [SerializeField]
        private GameObject _groupOfSelectables;

        [SerializeField]
        private UnityEvent<ClassSelectionInfo> _onUpdated;

        [SerializeField]
        private UnityEvent<ClassSelectionInfo, bool> _closingEvent;

        private bool _fullyLoaded;

        internal void UpdateDescription(ClassSelectionInfo classSelectionInfo)
        {
            if (_current != null) _current.GeneralObject.SetActive(false);
            else
            {
                _generalName.gameObject.SetActive(true);
                _selfEffect.gameObject.SetActive(true);
                _groupEffect.gameObject.SetActive(true);
            }

            if (classSelectionInfo != null)
            {
                _className.text = classSelectionInfo.ClassType.ToString();
                _description.text = classSelectionInfo.ClassDescription;
                _generalName.text = classSelectionInfo.GeneralName;
                _selfEffect.text = classSelectionInfo.SelfEffectDescription;
                _groupEffect.text = classSelectionInfo.GroupEffectDescription;

                _current = classSelectionInfo;
                UpdateGeneralObject(_current.GeneralObject);
            }
            else
            {
                _className.text = "Remove Army";
                _description.text = "Select this to remove current army";
                _generalName.gameObject.SetActive(false);
                _selfEffect.gameObject.SetActive(false);
                _groupEffect.gameObject.SetActive(false);

                _current = null;
            }
            _onUpdated.Invoke(classSelectionInfo);

        }
        public void Close(bool save) => _closingEvent.Invoke(_current, save);
        public void RemoveArmy() => _closingEvent.Invoke(null, true);
        private void Start()
        {
            var firstSelect = _groupOfSelectables.GetComponentInChildren<ClassSelectionInfo>();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            _current = firstSelect;
            _fullyLoaded = true;
        }
        private void OnEnable()
        {
            if (!_fullyLoaded) return;
            var firstSelect = _groupOfSelectables.GetComponentInChildren<ClassSelectionInfo>();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            _current = firstSelect;
        }
        private void OnDisable()
        {
            _current?.GeneralObject?.SetActive(false);
            _current = null;
        }
        private void UpdateGeneralObject(GameObject obj)
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector2((Screen.width - _statScreen.rect.width) / 2, Screen.height / 2));
            pos.x -= 5;
            pos.z = 0;
            obj.transform.position = pos;

            obj.SetActive(true);
        }
        private void OnDestroy()
        {
            _onUpdated.RemoveAllListeners();
        }
    }
}
