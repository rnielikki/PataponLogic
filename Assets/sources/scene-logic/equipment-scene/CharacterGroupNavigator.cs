using PataRoad.Common.Navigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterGroupNavigator : SpriteNavigator
    {

        private CameraZoom _cameraZoom;
        private CharacterGroupSaver _groupSaver;
        private Vector2[] _navPositions;

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
        [SerializeField]
        StatDisplay _statDisplay;

        public override void Init()
        {
            _map = GetComponent<ActionEventMap>();
            _cameraZoom = Camera.main.GetComponent<CameraZoom>();
            _groupSaver = GetComponent<CharacterGroupSaver>();

            _selectOnInit = false;
            base.Init();
            LoadClasses();
            _navPositions = _selectables.Select(n => (Vector2)n.transform.position).ToArray();
            Current.SelectThis();
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
                if (info != null)
                {
                    var targetNav = _selectables[_index].GetComponent<CharacterNavigator>();
                    Current?.OnDeselect(null);
                    RemoveChildren(targetNav);
                    AddTarget(info);
                    ReOrderIndex();

                    _audioSource.PlayOneShot(_soundIn);

                }
                else
                {
                    RemoveTarget();
                }
            }
            _groupSaver.Animate(gameObject);
            Current?.SelectThis();
        }
        public void RemoveTarget()
        {
            var nav = _selectables[_index].GetComponent<CharacterNavigator>();
            if (Core.GlobalData.PataponInfo.ClassCount > 1 && RemoveChildren(nav))
            {
                ReOrderIndex(99);
                _statDisplay.Empty();
                nav.Init();
                _audioSource.PlayOneShot(_soundOut);
            }
            else
            {
                _audioSource.PlayOneShot(_soundError);
            }
            ReOrderIndex(99);
        }
        private bool RemoveChildren(CharacterNavigator targetNav)
        {
            //1. remove old, if exists.-------------------------
            if (!targetNav.IsEmpty)
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
            Core.GlobalData.PataponInfo.AddClass(info.ClassType);

            _selectables[_index].GetComponent<CharacterNavigator>().Init();
        }
        private void LoadClasses()
        {
            var classes = Core.GlobalData.PataponInfo.CurrentClasses;
            for (int i = 0; i < Core.Character.Patapons.Data.PataponInfo.MaxPataponGroup; i++)
            {
                var targetNav = _selectables[i].GetComponent<CharacterNavigator>();
                if (i < classes.Length)
                {
                    var group = _groupSaver.LoadGroup(classes[i]);
                    group.transform.position = targetNav.transform.position;
                    group.transform.parent = targetNav.transform;
                }
                targetNav.Init();
            }
        }
        private void ReOrderIndex() => ReOrderIndex((int)_selectables[_index].GetComponentInChildren<Core.Character.PataponData>().Type);
        private void ReOrderIndex(int classTypeAsNumber)
        {
            int i;
            Dictionary<SpriteSelectable, (int oldIndex, int newIndex)> indexMap = new Dictionary<SpriteSelectable, (int oldIndex, int newIndex)>();
            int currentType = classTypeAsNumber;

            var indexOfCurrent = _index;

            for (i = 0; i < _selectables.Count; i++)
            {
                var pataponData = _selectables[i].GetComponentInChildren<Core.Character.PataponData>();
                int type = 99 + i;
                if (pataponData != null) type = (int)pataponData.Type;
                var selectable = _selectables[i];
                if (i < _index && type > currentType)
                {
                    indexMap.Add(selectable, (i, i + 1));
                    indexOfCurrent--;
                }
                else if (i > _index && type < currentType)
                {
                    indexMap.Add(selectable, (i, i - 1));
                    indexOfCurrent++;
                }
                else if (i != _index)
                {
                    indexMap.Add(selectable, (i, i));
                }
            }
            indexMap.Add(_selectables[_index], (_index, indexOfCurrent));

            ///reorder <see cref="_selectables"/>
            _selectables = indexMap.OrderBy(kv => kv.Value.newIndex).Select(kv => kv.Key).ToList();
            _index = indexOfCurrent;
            StartCoroutine(MoveAllPositions(indexMap));
        }
        IEnumerator MoveAllPositions(Dictionary<SpriteSelectable, (int oldIndex, int newIndex)> indexMap)
        {
            _map.enabled = false; //don't do anything while moving, prevents confusion and bug
            int waiting = 0;
            for (int i = 0; i < _selectables.Count; i++)
            {
                var selectable = _selectables[i];
                var (oldValue, newValue) = indexMap[selectable];
                if (oldValue != newValue)
                {
                    waiting++;
                    StartCoroutine(MovePosition(selectable.transform, _navPositions[i], Mathf.Abs(oldValue - newValue)));
                }
            }
            yield return new WaitUntil(() => waiting == 0);
            _map.enabled = true;

            IEnumerator MovePosition(Transform targetTransform, Vector3 newPosition, int speedOffset)
            {
                while (targetTransform.position != newPosition)
                {
                    targetTransform.position = Vector3.MoveTowards(targetTransform.position, newPosition, speedOffset * 15 * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                waiting--;
            }
        }

    }
}
