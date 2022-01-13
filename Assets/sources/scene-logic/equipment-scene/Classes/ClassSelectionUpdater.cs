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
        private EmptyClassSelection _emptySelection;
        private ClassSelectionInfo _current;
        [SerializeField]
        private GameObject _groupOfSelectables;
        [SerializeField]
        RectTransform _summaryTransform;
        RectTransform _summaryParentTransform;

        [SerializeField]
        private UnityEvent<ClassSelectionInfo> _onUpdated;

        [SerializeField]
        private UnityEvent<ClassSelectionInfo, bool> _closingEvent;

        private bool _fullyLoaded;

        private void Awake()
        {
            _summaryParentTransform = _summaryTransform.parent.GetComponent<RectTransform>();
        }
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
            Load();
            _fullyLoaded = true;
        }
        private void OnEnable()
        {
            if (!_fullyLoaded) return;
            Load();
        }
        private void OnDisable()
        {
            _current?.GeneralObject?.SetActive(false);
            _current = null;
        }
        private void Load()
        {
            var firstSelect = _groupOfSelectables.GetComponentInChildren<ClassSelectionInfo>();

            if (firstSelect != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
                UpdateDescription(firstSelect);
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current
                   .SetSelectedGameObject(_emptySelection.gameObject);
            }
            _current = firstSelect;
        }
        private void UpdateGeneralObject(GameObject obj)
        {
            var pos = Camera.main.ScreenToWorldPoint(
                new Vector2(
                _summaryTransform.anchoredPosition.x - _summaryParentTransform.anchoredPosition.x,
                Screen.height / 2
                )
            );
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
