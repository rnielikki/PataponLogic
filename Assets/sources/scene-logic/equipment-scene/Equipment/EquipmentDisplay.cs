using PataRoad.Common.Navigator;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentDisplay : MonoBehaviour
    {
        private ActionEventMap _map;
        private SpriteNavigator _nav;
        [SerializeField]
        private CharacterGroupNavigator _groupNav;
        [SerializeField]
        private GameObject _child;
        [SerializeField]
        private EquipmentSummary _summary;

        private RectTransform _rect;

        [SerializeField]
        private Text _text;

        private bool _doesNavPreserveIndex;

        // Start is called before the first frame update
        void Awake()
        {
            _map = GetComponent<ActionEventMap>();
            _map.enabled = false;
            _rect = _child.GetComponent<RectTransform>();
        }

        public void ShowEquipment(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as SpriteNavigator;
            var data = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (data == null) return;
            _nav.PreserveIndexOnDeselected = true;
            _doesNavPreserveIndex = false;
            var elem = _summary.Current;
            _summary.SetInactive();

            SetPosition(data.IndexInGroup < 2);

            SetAppear(LoadItemType(data, elem));

        }
        public void ShowItem(string itemType)
        {
            _nav = _groupNav;
            _doesNavPreserveIndex = true;
            SetAppear(itemType);
            SetPosition(false);
        }
        private void SetAppear(string itemType)
        {
            _map.enabled = true;
            enabled = true;
            _nav.Freeze();
            _child.SetActive(true);

            GetComponentInChildren<Selectable>().Select();
            _text.text = $"Type : {itemType}";
        }
        public void HideEquipment()
        {
            _nav.PreserveIndexOnDeselected = _doesNavPreserveIndex;
            _nav.Defrost();
            _map.enabled = false;
            _child.SetActive(false);

            _summary.ResumeToActive();
        }
        private string LoadItemType(Core.Character.PataponData data, EquipmentSummaryElement equipElement)
        {
            if (equipElement.IsGeneralMode) return "Key/GeneralMode";
            else return "Equipment/" + data.GetEquipmentName(equipElement.EquipmentType);
        }
        private void SetPosition(bool left)
        {
            Vector2 pos;
            if (left) pos = Vector2.zero;
            else pos = Vector2.right;
            _rect.anchorMin = pos;
            _rect.anchorMax = pos;
        }
    }
}
