using PataRoad.Common.Navigator;
using System;
using System.Linq;
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
        [SerializeField]
        private GameObject _equipmentSummaryField;
        [SerializeField]
        AudioClip _soundIn;
        [SerializeField]
        AudioClip _soundOut;
        [SerializeField]
        AudioClip _soundError;
        [SerializeField]
        ClassMenu _classMenu;

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
                _equipmentSummaryField.SetActive(true);
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
            if (saved)
            {
                Current?.OnDeselect(null);
                RemoveChildren();
                AddTarget(info);
                ReOrderIndex();

                _audioSource.PlayOneShot(_soundIn);
                _classMenu.UpdateStatus();
            }
            _groupSaver.Animate(gameObject);
            Current?.SelectThis();
        }
        public void RemoveTarget()
        {
            _audioSource.PlayOneShot(RemoveChildren() ? _soundOut : _soundError);
        }
        private bool RemoveChildren()
        {
            //1. remove old, if exists.-------------------------
            if (!_navigators[_index].IsEmpty)
            {
                var oldObject = GetPataponGroupObject(Current);
                oldObject.transform.parent = transform.root.parent;
                oldObject.SetActive(false);
                Core.GlobalData.PataponInfo.RemoveClass(
                    oldObject.GetComponentInChildren<Core.Character.PataponData>().Type
                );
                return true;
            }
            else
            {
                return false;
            }
        }
        private GameObject GetPataponGroupObject(SpriteSelectable target) => target.transform.Find("PataponGroup")?.gameObject;
        private void AddTarget(ClassSelectionInfo info)
        {
            //2. add new-----------------------------------
            var targetGroupObject = info.GroupObject;
            targetGroupObject.transform.parent = Current.transform;
            targetGroupObject.transform.position = Current.transform.position;
            targetGroupObject.SetActive(true);
            _navigators[_index].Init();
            //update data
            Core.GlobalData.PataponInfo.AddClass(info.ClassType);
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
        private void ReOrderIndex()
        {
            //this is before ordering index.
            var oldIndex = _index;
            var allGroupObjects = _selectables.Select(s => GetPataponGroupObject(s)).Where(s => s != null);
            var allGroupObjectsToArray = allGroupObjects.ToArray();

            //this is right index.
            var ordered = allGroupObjects.OrderBy(s => s.GetComponentInChildren<Core.Character.PataponData>().Type).ToArray();
            foreach (var current in allGroupObjects)
            {
                var indexOld = Array.IndexOf(allGroupObjectsToArray, current);
                var indexNew = Array.IndexOf(ordered, current);
                if (indexOld < 0 || indexNew < 0) throw new System.InvalidOperationException("WTF");
                MoveGroupTo(current, indexOld, indexNew);
                if (oldIndex == indexOld)
                {
                    _index = indexNew;
                }
            }
            //Finally, match to right index (note that it's not in progress. If you do in progress it'll cause bug.)
            foreach (var comp in GetComponentsInChildren<CharacterNavigator>()) comp.Init();
        }
        private void MoveGroupTo(GameObject pataponGroup, int beforeIndex, int newIndex)
        {
            if (beforeIndex != newIndex)
            {
                var targetTransform = _navigators[newIndex].transform;
                pataponGroup.transform.parent = targetTransform;
                pataponGroup.transform.position = targetTransform.position;
            }
        }
    }
}
