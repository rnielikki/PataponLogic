using UnityEngine;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class HeadquarterMenu : SummaryMenu<HeadquarterSummaryElement>
    {
        [SerializeField]
        HeadquarterSummaryElement _equipElement;
        private bool _hideEquipElement;
        private void Awake()
        {
            Init();
            _activeNavs = GetComponentsInChildren<HeadquarterSummaryElement>();
        }
        private void OnEnable()
        {
            MarkIndex(_index);
            _actionEvent.enabled = true;
        }
        public void HideIfEmpty(Common.Navigator.SpriteSelectable sender)
        {
            if (sender.GetComponent<CharacterNavigator>().IsEmpty)
            {
                if (Current == _equipElement) MarkIndex((_index + 1) % _activeNavs.Length);
                _hideEquipElement = true;
            }
            else
            {
                _hideEquipElement = false;
            }
            _equipElement.gameObject.SetActive(!_hideEquipElement);
        }
        public void Submit()
        {
            Current.OnSubmit.Invoke(Current);
        }
        public override void ResumeToActive()
        {
            base.ResumeToActive();
            Current.MarkAsSelected();
        }
        public override void MoveTo(Object sender, InputAction.CallbackContext context)
        {
            base.MoveTo(sender, context);
            if (_hideEquipElement && Current == _equipElement)
            {
                var directionY = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
                MoveTo(directionY);
            }
        }
    }
}
