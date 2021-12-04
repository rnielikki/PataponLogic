using PataRoad.Common.Navigator;
using PataRoad.Core.Character.Equipments;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSummary : MonoBehaviour
    {
        [SerializeField]
        AudioSource _selectSoundSource;
        [SerializeField]
        AudioClip _selectSound;
        ActionEventMap _actionEvent;

        private Dictionary<EquipmentType, EquipmentSummaryElement> _map;
        private EquipmentSummaryElement _generalMode;

        //It really selects nothing. just look like it's selected.
        private int _index;
        private EquipmentSummaryElement[] _activeNavs;
        private bool _hasGeneral;
        public EquipmentSummaryElement Current { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            _actionEvent = GetComponent<ActionEventMap>();
            _map = new Dictionary<EquipmentType, EquipmentSummaryElement>();
            foreach (var obj in GetComponentsInChildren<EquipmentSummaryElement>())
            {
                if (obj.IsGeneralMode)
                {
                    _generalMode = obj;
                }
                else
                {
                    _map.Add(obj.EquipmentType, obj);
                }
                obj.gameObject.SetActive(false);
            }
            _actionEvent.enabled = false;
        }
        private void OnDisable()
        {
            SetInactive();
            Current?.MarkAsDeselected();
            _index = 0;
        }
        public void SetInactive()
        {
            if (_activeNavs != null)
            {
                foreach (var elem in _activeNavs)
                {
                    elem.enabled = false;
                }
            }
            _actionEvent.enabled = false;
        }
        public void ResumeToActive()
        {
            if (_activeNavs != null)
            {
                Current?.MarkAsDeselected();
                foreach (var elem in _activeNavs)
                {
                    elem.enabled = true;
                }
                SelectSameOrZero();
            }
            _actionEvent.enabled = true;
        }

        public void LoadElements(SpriteSelectable target)
        {
            var ponData = target.GetComponent<Core.Character.PataponData>();
            var equipmentManager = ponData?.EquipmentManager;
            if (equipmentManager == null) return;

            foreach (var kvPair in _map)
            {
                (EquipmentType equipmentType, EquipmentSummaryElement row) = (kvPair.Key, kvPair.Value);
                var currentData = equipmentManager.GetEquipmentData(equipmentType);
                if (currentData == null)
                {
                    row.gameObject.SetActive(false);
                }
                else
                {
                    row.gameObject.SetActive(true);
                    row.SetText(currentData.Name);
                }
            }
            _generalMode.gameObject.SetActive(ponData.IsGeneral);
            _hasGeneral = ponData.IsGeneral;

            //initialize navigation
            _activeNavs = GetComponentsInChildren<EquipmentSummaryElement>(false);
            SelectSameOrZero();

            if (!_actionEvent.enabled) _actionEvent.enabled = true;
        }
        public void MoveTo(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (EventSystem.current.alreadySelecting) return;

            var directionY = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
            var index = _index;

            if (directionY == 1 || directionY == -1)
            {
                index = (index + directionY * -1 + _activeNavs.Length) % _activeNavs.Length;
            }
            MarkIndex(index);
            if (_selectSound != null) _selectSoundSource.PlayOneShot(_selectSound);
        }
        private void MarkIndex(int index)
        {
            var oldCurrent = Current;
            oldCurrent?.MarkAsDeselected();
            _index = index;
            Current = _activeNavs[_index];
            Current.MarkAsSelected();
        }
        private void SelectSameOrZero()
        {
            int index = 0;
            if (Current != null)
            {
                index = System.Array.IndexOf(_activeNavs, Current);
                if (index < 0) index = 0;
            }
            MarkIndex(index);
        }
    }
}
