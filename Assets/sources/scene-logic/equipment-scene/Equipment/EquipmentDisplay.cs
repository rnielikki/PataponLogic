using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentDisplay : MonoBehaviour
    {
        private Common.Navigator.ActionEventMap _map;
        private CharacterNavigator _nav;
        [SerializeField]
        private GameObject _child;
        [SerializeField]
        private EquipmentSummary _summary;

        private RectTransform _rect;

        [SerializeField]
        private Text _text;

        // Start is called before the first frame update
        void Awake()
        {
            _map = GetComponent<Common.Navigator.ActionEventMap>();
            _map.enabled = false;
            _rect = _child.GetComponent<RectTransform>();
        }

        // Update is called once per frame
        public void ShowEquipment(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as CharacterNavigator;
            var data = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (data == null) return;

            _nav.PreserveIndexOnDeselected = true;
            _map.enabled = true;
            enabled = true;
            _nav.Freeze();
            _child.SetActive(true);

            var elem = _summary.Current;
            _summary.SetInactive();

            GetComponentInChildren<Selectable>().Select();
            _text.text = $"Type : {LoadItemType(data, elem)}";

            Vector2 pos;

            if (data.IndexInGroup > 1)
            {
                pos = Vector2.right;
            }
            else
            {
                pos = Vector2.zero;
            }
            _rect.anchorMin = pos;
            _rect.anchorMax = pos;

        }
        public void HideEquipment(Object sender)
        {
            _nav.enabled = true;
            _nav.PreserveIndexOnDeselected = false;
            _nav.Current.SelectThis();
            _map.enabled = false;
            _child.SetActive(false);

            _summary.ResumeToActive();
        }
        private string LoadItemType(Core.Character.PataponData data, EquipmentSummaryElement equipElement)
        {
            if (equipElement.IsGeneralMode) return "Key/GeneralMode";
            else return "Equipment/" + data.GetEquipmentName(equipElement.EquipmentType);
        }
    }
}
