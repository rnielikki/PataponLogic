using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class AttackTypeSelector : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _current;
        [SerializeField]
        GameObject _attackTypeMenuElement;
        [SerializeField]
        Common.Navigator.ActionEventMap _actionEvent;

        [SerializeField]
        private CharacterGroupNavigator _groupNav;
        private CharacterGroupData _groupData;
        public void LoadElements()
        {
            _groupData = _groupNav.Current.GetComponentInChildren<CharacterGroupData>();
            if (_groupData == null) return;
            gameObject.SetActive(true);
            LoadElements(_groupData.ClassData.AvailableAttackTypes, Core.Global.GlobalData.CurrentSlot.PataponInfo.GetAttackTypeIndex(_groupData.Type));
        }
        private void LoadElements(string[] items, int currentIndex)
        {
            _current.text = items[currentIndex];
            for (int i = 0; i < items.Length; i++)
            {
                var obj = Instantiate(_attackTypeMenuElement, transform);
                var elem = obj.GetComponent<AttackTypeMenuElement>();
                elem.Init(i, items[i], SaveAndClose);
                if (i == currentIndex) UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(obj);
            }
        }
        public void SaveAndClose(int index)
        {
            Core.Global.GlobalData.CurrentSlot.PataponInfo.SetAttackTypeIndex(_groupData.Type, index);
            CloseWithoutSaving();
        }
        public void CloseWithoutSaving()
        {
            _groupData = null;
            Core.Global.GlobalData.Sound.PlaySelected();
            gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            _groupNav.Freeze();
            _actionEvent.enabled = true;
        }
        private void OnDisable()
        {
            foreach (var elem in GetComponentsInChildren<AttackTypeMenuElement>()) Destroy(elem.gameObject);
            _actionEvent.enabled = false;
            _groupNav.Defrost();
        }
    }
}
