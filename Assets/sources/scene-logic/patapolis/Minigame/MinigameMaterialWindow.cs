using PataRoad.Core.Items;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameMaterialWindow : MonoBehaviour
    {
        MinigameSelectionWindow _parent;
        [SerializeField]
        GameObject _template;
        [SerializeField]
        Transform _contentTarget;
        [SerializeField]
        UnityEngine.UI.VerticalLayoutGroup _childTargetToRefresh;
        [SerializeField]
        UnityEngine.UI.VerticalLayoutGroup _targetToRefresh;
        private MinigameSelectionButton _lastSelection;
        [SerializeField]
        UnityEngine.UI.Text _estimationText;
        private float _estimation;
        [SerializeField]
        private UnityEngine.UI.Button _okButton;
        [SerializeField]
        Common.Navigator.HorizontalNavigationGroup _navGroup;
        [SerializeField]
        [Tooltip("Although it says it's animation curve, it's not related to animation at all.")]
        AnimationCurve _estimationCurve;
        MaterialLoader[] _materialLoaders;

        public void Open(MinigameSelectionWindow parent, MinigameSelectionButton model)
        {
            _lastSelection = model;
            _parent = parent;
            _parent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                foreach (var group in model.MaterialGroups)
                {
                    var obj = Instantiate(_template, _contentTarget);
                    obj.GetComponent<MaterialLoader>().Init(group, this);
                }
                //Canvas update ------------------
                Canvas.ForceUpdateCanvases();
                _childTargetToRefresh.enabled = false;
                _targetToRefresh.enabled = false;
                _childTargetToRefresh.enabled = true;
                _targetToRefresh.enabled = true;

                _navGroup.LateInit();
                _materialLoaders = _contentTarget.GetComponentsInChildren<MaterialLoader>();
                foreach (var materialLoader in _materialLoaders)
                {
                    materialLoader.LateInit();
                }
                UpdateEstimation();
                _okButton.Select();
            }));
        }
        public void Close(bool ok = false)
        {
            _parent.gameObject.SetActive(true);
            gameObject.SetActive(false);
            foreach (Transform child in _contentTarget)
            {
                Destroy(child.gameObject);
            }
            if (!ok)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_lastSelection.gameObject);
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
        public void LoadGame()
        {
            // UI/Start to start.
            if (_materialLoaders.Any(loader => loader.Item == null))
            {
                Core.Global.GlobalData.Sound.PlayBeep();
                Common.GameDisplay.ConfirmDialog.CreateCancelOnly("Item is insufficient", this);
                return;
            }
            var window = Common.GameDisplay.ConfirmDialog.Create("You will play in REAL.\nIf you fail, item will be LOST without reward.\nAre you sure to play in this mode?", this, () =>
            {
                foreach (var materialLoader in _materialLoaders)
                {
                    Core.Global.GlobalData.Inventory.RemoveItem(materialLoader.Item);
                }
                //Load scene!
            });
            window.IsScreenChange = true;
        }

        internal void UpdateEstimation()
        {
            if (_materialLoaders.Any(loader => loader.Item == null))
            {
                _estimationText.text = "-";
                return;
            }
            _estimation = _estimationCurve.Evaluate((float)_materialLoaders.Average(value => value.Item.Index) / 8);
            _estimationText.text = _estimation.ToString("p0");
        }

        public void LoadPracticeGame()
        {
            var window = Common.GameDisplay.ConfirmDialog.Create("You're play as PRACTICE.\nThere will be NO REWARD, but NO ITEM WILL BE LOST.\nAre you sure to play in this mode?", this, () =>
            {
                //LOAD SCENE!
            });
            window.IsScreenChange = true;
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
    }
}
