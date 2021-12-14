using PataRoad.Common.Navigator;
using PataRoad.Core.Character.Equipments;
using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentSummary : SummaryMenu<EquipmentSummaryElement>
    {
        private Dictionary<EquipmentType, EquipmentSummaryElement> _map;
        private EquipmentSummaryElement _generalMode;
        [SerializeField]
        private EquipmentSummaryElement _helmElement;
        private SpriteSelectable _currentTarget;

        //It really selects nothing. just look like it's selected.

        void Awake()
        {
            Init();
            _actionEvent.enabled = false;
            _map = new Dictionary<EquipmentType, EquipmentSummaryElement>();
            foreach (var obj in GetComponentsInChildren<EquipmentSummaryElement>(true))
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
        }
        public override void ResumeToActive()
        {
            if (_activeNavs != null)
            {
                base.ResumeToActive();
                SelectSameOrZero();
            }
        }
        private void HideHelmIfRarepon(EquipmentManager equipmentManager)
        {
            if (equipmentManager.Rarepon != null && equipmentManager.Rarepon.CurrentData.Index != 0)
            {
                _helmElement.gameObject.SetActive(false);
            }
        }
        public void LoadElements(SpriteSelectable target)
        {
            var ponData = target.GetComponent<Core.Character.PataponData>();
            var equipmentManager = ponData.EquipmentManager;

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
                    row.SetItem(currentData);
                }
            }
            _generalMode.gameObject.SetActive(ponData.IsGeneral);
            if (ponData.IsGeneral)
            {
                _generalMode.SetItem(
                    Core.Global.GlobalData.PataponInfo.GetGeneralMode(ponData.Type)
                    );
            }
            HideHelmIfRarepon(equipmentManager);

            //initialize navigation
            _activeNavs = GetComponentsInChildren<EquipmentSummaryElement>(false);
            SelectSameOrZero();

            if (!_actionEvent.enabled) _actionEvent.enabled = true;
            _currentTarget = target;
        }
        protected override void WhenDisabled()
        {
            _index = 0;
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
        public void UpdateAll(bool isGroup)
        {
            if (isGroup) return;
            foreach (var nav in _activeNavs)
            {
                if (nav.Item?.ItemType == Core.Items.ItemType.Equipment)
                {
                    var type = (nav.Item as Core.Items.EquipmentData).Type;
                    var target = _currentTarget.GetComponent<Core.Character.PataponData>();
                    nav.SetItem(target.EquipmentManager.GetEquipmentData(type));
                }
            }
        }
    }
}
