using PataRoad.Common.Navigator;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterGroupNavigator : SpriteNavigator
    {
        [SerializeField]
        private Sprite _characterBackground;
        [SerializeField]
        private AudioClip _childSelectionAudioClip;
        [SerializeField]
        private UnityEvent<SpriteSelectable> _onChildSelected;
        [SerializeField]
        private UnityEvent<Object, UnityEngine.InputSystem.InputAction.CallbackContext> _onChildSubmit;
        private CameraZoom _cameraZoom;
        public override void Init()
        {
            base.Init();
            _map = GetComponent<ActionEventMap>();
            _cameraZoom = Camera.main.GetComponent<CameraZoom>();
            foreach (var nav in _navs)
            {
                var navigator = nav.gameObject.AddComponent<CharacterNavigator>();
                navigator.Init(_characterBackground, _audioSource, _childSelectionAudioClip, _onChildSelected, _onChildSubmit);
                navigator.enabled = false;
            }
            _navs[0].SelectThis();
        }
        public void Zoom()
        {
            var charNavigator = Current.GetComponent<CharacterNavigator>();
            SelectOther(charNavigator, () => ZoomIn(charNavigator.transform));
        }
        public void ResumeFromZoom()
        {
            _cameraZoom.ZoomOut();

            foreach (var nav in _navs)
            {
                var pos = nav.transform.position;
                pos.z = 0;
                nav.transform.position = pos;
            }
        }

        private void ZoomIn(Transform targetTransform)
        {
            _cameraZoom.ZoomIn(targetTransform);
            foreach (var nav in _navs)
            {
                if (nav.gameObject != Current.gameObject)
                {
                    //Deactivating can cause incorrect action so hiding by moving out of camera position sight.
                    var pos = nav.transform.position;
                    pos.z = _cameraZoom.transform.position.z - 10;
                    nav.transform.position = pos;
                }
            }
        }
    }
}
