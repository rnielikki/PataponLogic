using PataRoad.Common.Navigator;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterNavigator : SpriteNavigator
    {
        private CharacterGroupNavigator _parent;
        public void Init(Sprite background, AudioSource audioSource, AudioClip selectSound,
            UnityEvent<SpriteSelectable> onSelected, UnityEvent<Object, UnityEngine.InputSystem.InputAction.CallbackContext> onSubmit)
        {
            _useSprite = true;
            _background = background;
            _audioSource = audioSource;
            _selectSound = selectSound;
            _onSelected = onSelected;

            _parent = GetComponentInParent<CharacterGroupNavigator>();
            Init();
            var onCanceled = new UnityEvent<Object, UnityEngine.InputSystem.InputAction.CallbackContext>();
            onCanceled.AddListener(SelectParent);

            _map = gameObject.AddComponent<ActionEventMap>();
            _map.SetSender(this);
            _map._actionAndEvents = new[]
            {
                new ActionEventMap.AEPair
                {
                    ActionName = "UI/Submit",
                    OnPerformed = onSubmit
                },
                new ActionEventMap.AEPair
                {
                    ActionName = "UI/Cancel",
                    OnPerformed = onCanceled
                }
            };
        }
        public override void Init()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponentInChildren<Core.Character.PataponData>() == null) continue;

                var comp = child.gameObject.AddComponent<SpriteSelectable>();
                if (_useSprite) comp.Init(this, _background, _positionOffset, _onSelected);
                else comp.Init(this, _backgroundObject, _positionOffset, _onSelected);
                _navs.Add(comp);
            }
        }
        private void SelectParent(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
            => SelectOther(_parent, _parent.ResumeFromZoom);
    }
}
