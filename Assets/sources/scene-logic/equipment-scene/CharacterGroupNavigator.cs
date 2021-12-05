using PataRoad.Common.Navigator;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterGroupNavigator : SpriteNavigator
    {

        private CameraZoom _cameraZoom;
        private CharacterGroupSaver _groupSaver;
        private CharacterNavigator[] _navigators;
        [SerializeField]
        private GameObject _summaryField;

        public override void Init()
        {
            _map = GetComponent<ActionEventMap>();
            _cameraZoom = Camera.main.GetComponent<CameraZoom>();
            _groupSaver = GetComponent<CharacterGroupSaver>();

            _navigators = GetComponentsInChildren<CharacterNavigator>();
            UpdateClasses();
            base.Init();
        }
        public void Zoom()
        {
            var charNavigator = Current.GetComponent<CharacterNavigator>();
            if (!charNavigator.IsEmpty)
            {
                SelectOther(charNavigator, () => ZoomIn(charNavigator.transform));
                _summaryField.SetActive(false);
            }
        }
        public void ResumeFromZoom()
        {
            _cameraZoom.ZoomOut();

            foreach (var nav in _selectables)
            {
                var pos = nav.transform.position;
                pos.z = 0;
                nav.transform.position = pos;
            }
        }

        private void ZoomIn(Transform targetTransform)
        {
            _cameraZoom.ZoomIn(targetTransform);
            foreach (var nav in _selectables)
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
        public void SelectCurrent(ClassSelectionInfo info, bool saved)
        {
            Current?.SelectThis();
            _groupSaver.Animate(gameObject);
            if (saved)
            {
                //update data
            }
        }
        public void UpdateClasses()
        {
            var classes = Core.GlobalData.PataponInfo.CurrentClasses;
            for (int i = 0; i < Core.Character.Patapons.Data.PataponInfo.MaxPataponGroup; i++)
            {
                var charNav = _navigators[i];
                if (i < classes.Length)
                {
                    var group = _groupSaver.LoadGroup(classes[i]);
                    group.transform.position = charNav.transform.position;
                    group.transform.parent = charNav.transform;
                }
                charNav.Init();
            }
        }
        public void PlaceGroup(int index)
        {
        }
        public void RemoveGroup()
        {
        }
    }
}
