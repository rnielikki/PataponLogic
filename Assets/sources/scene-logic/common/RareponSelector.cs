using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.SceneLogic.EquipmentScene;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    internal class RareponSelector : MonoBehaviour
    {
        [SerializeField]
        UnityEvent _onOpen;
        [SerializeField]
        UnityEvent<RareponSelection> _onClicked;
        [SerializeField]
        UnityEvent _onClosed;
        [SerializeField]
        AudioClip _newRareponSound;
        public AudioClip NewRareponSound => _newRareponSound;
        private Core.Character.PataponData _targetPatapon;
        private Common.Navigator.SpriteNavigator _lastSelectNavigator;

        [SerializeField]
        private EquipmentSetter _equipmentSetter;
        private RareponSelection[] _rareponSelections;
        [SerializeField]
        private Common.Navigator.ActionEventMap _actionEventMap;

        private RareponSelection _lastSelected;

        [SerializeField]
        private Text _descriptionTitle;
        [SerializeField]
        private Text _descriptionContent;

        public void Open(Common.Navigator.SpriteNavigator beforeSelect, Core.Character.PataponData pataponData, RareponData data)
        {
            _targetPatapon = pataponData;
            _lastSelectNavigator = beforeSelect;
            gameObject.SetActive(true);
            beforeSelect.Freeze();
            Open(data);
        }
        public void Open(RareponData data)
        {
            _actionEventMap.enabled = true;
            _rareponSelections = GetComponentsInChildren<RareponSelection>();
            foreach (var rareponSelection in _rareponSelections)
            {
                rareponSelection.GetComponent<Button>()
                    .onClick
                    .AddListener(() => _onClicked.Invoke(rareponSelection));
            }
            _onOpen.Invoke();
            gameObject.SetActive(true);
            Select(data);
        }
        public void UpdateText(RareponData data)
        {
            if (data == null)
            {
                _descriptionTitle.text = "???";
                _descriptionContent.text = "Select to reveal the Rarepon";
            }
            else
            {
                _descriptionTitle.text = data.Name;
                _descriptionContent.text = data.Description;
            }
        }

        private void OnEnable()
        {
            if (_lastSelected == null) return;
            foreach (var selection in _rareponSelections)
            {
                selection.enabled = true;
            }
            _lastSelected.Select();
            _actionEventMap.enabled = true;
        }

        private void OnDisable()
        {
            var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            foreach (var selection in _rareponSelections)
            {
                if (selection.gameObject == currentSelected) _lastSelected = selection;
                selection.enabled = false;
            }
            _actionEventMap.enabled = false;
        }
        public void Select(RareponData data)
        {
            var index = data.Index;
            var uiOkAction = Core.Global.GlobalData.Input.actions.FindAction("UI/Submit");
            uiOkAction.Disable();
            StartCoroutine(WaitForNext());

            System.Collections.IEnumerator WaitForNext()
            {
                if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate)
                {
                    yield return new WaitForEndOfFrame();
                }
                else if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsInFixedUpdate)
                {
                    yield return new WaitForFixedUpdate();
                }
                _rareponSelections.SingleOrDefault(s => s.Index == index)?.Select();
                uiOkAction.Enable();
            }
        }
        public void Apply(RareponSelection selection)
        {
            if (selection.RareponData == null)
            {
                CreateRarepon(selection);
            }
            else
            {
                _equipmentSetter.SetEquipment(_targetPatapon, selection.RareponData);
                Close();
            }
        }
        public void CreateNewRarepon(RareponSelection selection)
        {
            if (selection.RareponData == null)
            {
                CreateRarepon(selection);
            }
        }
        private void CreateRarepon(RareponSelection selection) => selection.ConfirmToCreateRarepon();

        public void Close()
        {
            _actionEventMap.enabled = false;
            gameObject.SetActive(false);
            Core.Global.GlobalData.Sound.PlaySelected();
            _lastSelectNavigator?.Defrost();
            _onClosed?.Invoke();
        }
    }
}
