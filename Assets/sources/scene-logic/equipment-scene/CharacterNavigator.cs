using PataRoad.Common.Navigator;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterNavigator : SpriteNavigator
    {
        private CharacterGroupNavigator _parent;
        [SerializeField]
        private EquipmentSummary _equipmentSummary;
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// Also can used for refreshing.
        /// </summary>
        public override void Init()
        {
            if (_parent == null) _parent = GetComponentInParent<CharacterGroupNavigator>();
            if (_map == null) _map = GetComponent<ActionEventMap>();

            _selectables.Clear();
            foreach (var comp in GetComponentsInChildren<SpriteSelectable>())
            {
                if (comp.gameObject == gameObject) continue;
                comp.Init(this);
                _selectables.Add(comp);
            }
            IsEmpty = _selectables.Count == 0;
        }
        public void SelectParent(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
            => SelectOther(_parent, _parent.ResumeFromZoom);
        protected override void OnThisEnabled()
        {
            _equipmentSummary.enabled = true;
        }
        protected override void OnThisDisabled()
        {
            if (_equipmentSummary != null) _equipmentSummary.enabled = false;
        }
    }
}
