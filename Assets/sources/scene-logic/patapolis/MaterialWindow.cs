using PataRoad.Core.Items;
using PataRoad.SceneLogic.Patapolis.Minigame;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    public abstract class MaterialWindow : MonoBehaviour
    {
        protected SelectionWindow _parent;
        private bool _opening;

        [SerializeField]
        AudioClip _openSound;
        [SerializeField]
        AudioClip _closeSound;

        [SerializeField]
        protected UnityEngine.UI.Button _okButton;

        [SerializeField]
        protected GameObject _template;
        [SerializeField]
        protected Transform _contentTarget;
        [SerializeField]
        protected UnityEngine.UI.VerticalLayoutGroup _targetToRefresh;
        [SerializeField]
        protected UnityEngine.UI.VerticalLayoutGroup _childTargetToRefresh;
        protected MaterialLoader[] _materialLoaders;

        private readonly System.Collections.Generic.List<Transform> _doNotDestroy
            = new System.Collections.Generic.List<Transform>();

        protected IMaterialSelectionButton _lastSelection;

        private void Start()
        {
            foreach (Transform child in transform)
            {
                _doNotDestroy.Add(child);
            }
        }

        protected virtual void OpenWindow(SelectionWindow parent, IMaterialSelectionButton button)
        {
            _lastSelection = button;
            _opening = true;
            _parent = parent;
            _parent.gameObject.SetActive(false);
            gameObject.SetActive(true);

            Core.Global.GlobalData.Sound.PlayInScene(_openSound);

            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                int index = 0;
                foreach (var group in button.MaterialGroups)
                {
                    var obj = Instantiate(_template, _contentTarget);
                    obj.GetComponent<MaterialLoader>().Init(group, this);
                    obj.transform.SetSiblingIndex(index++);
                }
                foreach (var navGroup in GetComponentsInChildren<Common.Navigator.HorizontalNavigationGroup>())
                {
                    navGroup.LateInit();
                }
                //Canvas update ------------------
                Canvas.ForceUpdateCanvases();
                _childTargetToRefresh.enabled = false;
                _targetToRefresh.enabled = false;
                _childTargetToRefresh.enabled = true;
                _targetToRefresh.enabled = true;

                _materialLoaders = _contentTarget.GetComponentsInChildren<MaterialLoader>();
                foreach (var materialLoader in _materialLoaders)
                {
                    materialLoader.LateInit();
                }
                UpdateEstimation();
                _okButton.Select();
                _opening = false;
            }));
        }
        internal abstract void UpdateEstimation();
        public void Close(bool ok = false)
        {
            if (_opening) return;
            _parent.gameObject.SetActive(true);
            gameObject.SetActive(false);
            if (!ok) Core.Global.GlobalData.Sound.PlayInScene(_closeSound);
            foreach (Transform child in _contentTarget)
            {
                if (!_doNotDestroy.Contains(child)) Destroy(child.gameObject);
            }
            if (!ok)
            {
                UnityEngine.EventSystems.EventSystem.current
                    .SetSelectedGameObject(_lastSelection.Button.gameObject);
            }
        }
        internal void RemoveOne(IItem item)
        {
            foreach (var materialLoader in _materialLoaders)
            {
                materialLoader.UpdateAmount(item, true);
            }
        }
        internal void RestoreOne(IItem item)
        {
            foreach (var materialLoader in _materialLoaders)
            {
                materialLoader.UpdateAmount(item, false);
            }
        }
        internal void FindNextSelectionTarget(MaterialLoader currentLoader)
        {
            foreach (var materialLoader in _materialLoaders)
            {
                if (materialLoader != currentLoader)
                {
                    var target = materialLoader.GetNextSelectionTarget();
                    if (target != null)
                    {
                        target.Selectable.Select();
                        return;
                    }
                }
            }
            //nothing to select!
            _okButton.Select();
        }
        protected bool PayMaterials()
        {
            // UI/Start to start.
            if (_materialLoaders.Any(loader => loader.Item == null))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                Common.GameDisplay.ConfirmDialog.Create("Item is insufficient")
                    .HideOkButton()
                    .SetTargetToResume(this)
                    .SelectCancel();
                return false;
            }
            return true;
        }
    }
}