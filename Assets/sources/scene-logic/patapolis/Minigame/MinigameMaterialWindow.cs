using PataRoad.Core.Items;
using PataRoad.SceneLogic.Minigame;
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
        [SerializeField]
        AudioClip _openSound;
        [SerializeField]
        AudioClip _closeSound;
        [SerializeField]
        AudioClip _startSound;
        MaterialLoader[] _materialLoaders;

        public MinigameData GameData => _lastSelection.MinigameData;
        private bool _opening;

        public void Open(MinigameSelectionWindow parent, MinigameSelectionButton button)
        {
            _opening = true;
            _lastSelection = button;
            _parent = parent;
            _parent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            Core.Global.GlobalData.Sound.PlayInScene(_openSound);
            StartCoroutine(
            Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                foreach (var group in button.MaterialGroups)
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
                _opening = false;
            }));
        }
        public void Close(bool ok = false)
        {
            if (_opening) return;
            _parent.gameObject.SetActive(true);
            gameObject.SetActive(false);
            if (!ok) Core.Global.GlobalData.Sound.PlayInScene(_closeSound);
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
            Common.GameDisplay.ConfirmDialog.Create("You will play in REAL.\nIf you fail, item will be LOST without reward.\nAre you sure to play in this mode?", this, () =>
            {
                int sum = 0;
                int difference = 0;
                foreach (var materialLoader in _materialLoaders)
                {
                    Core.Global.GlobalData.Inventory.RemoveItem(materialLoader.Item);
                }
                //Load scene!
                var (item, amount) = _lastSelection.GetReward(_materialLoaders.Sum(lo => lo.Item.Index), _materialLoaders.Max(lo => lo.Item.Index) - _materialLoaders.Min(lo => lo.Item.Index));
                LoadMinigaeScene(new MinigameModel(_lastSelection.MinigameData, _estimation, item, amount));
            });
        }

        internal void UpdateEstimation()
        {
            if (_materialLoaders.Any(loader => loader.Item == null))
            {
                _estimationText.text = "-";
                return;
            }
            _estimation = _estimationCurve.Evaluate((float)_materialLoaders.Average(value => value.Item.Index) / 8);
            _estimationText.text = _estimation.ToString("p2");
        }

        public void LoadPracticeGame()
        {
            Common.GameDisplay.ConfirmDialog.Create("You're play as PRACTICE.\nThere will be NO REWARD, but item WILL NOT LOST.\nAre you sure to play in this mode?", this, () =>
            {
                //LOAD SCENE!
                LoadMinigaeScene(new MinigameModel(_lastSelection.MinigameData));
            });
        }
        private void LoadMinigaeScene(MinigameModel model)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_startSound);
            Close(true);
            _parent.Close(true);
            MinigameManager.Init(model);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Minigame", UnityEngine.SceneManagement.LoadSceneMode.Additive);
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
