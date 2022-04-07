using PataRoad.SceneLogic.Minigame;
using System.Linq;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    class MinigameMaterialWindow : MaterialWindow
    {
        private MinigameSelectionButton _lastMinigameSelection =>
            _lastSelection as MinigameSelectionButton;
        [SerializeField]
        UnityEngine.UI.Text _estimationText;
        AnimationCurve _estimationCurve;
        private float _estimation;
        [SerializeField]
        AudioClip _startSound;

        public MinigameData GameData => _lastMinigameSelection.MinigameData;

        public void Open(MinigameSelectionWindow parent, MinigameSelectionButton button)
        {
            _estimationCurve = parent.RequirementCurve;
            OpenWindow(parent, button);
        }
        public void LoadGame()
        {
            PayMaterials();
            Common.GameDisplay.ConfirmDialog.Create("You will play in REAL.\n"
                + "If you fail, item will be LOST without reward.\n"
                + "You can practice instead without losing item.\nAre you sure to play in this mode?")
                .SetTargetToResume(this)
                .SetOkAction(() =>
                {
                    foreach (var materialLoader in _materialLoaders)
                    {
                        Core.Global.GlobalData.CurrentSlot.Inventory.RemoveItem(materialLoader.Item);
                    }
                    //Load scene!
                    var (item, amount) = _lastMinigameSelection.GetReward(
                        _materialLoaders.Sum(lo => lo.Item.Index),
                        _materialLoaders.Max(lo => lo.Item.Index) - _materialLoaders.Min(lo => lo.Item.Index));
                    LoadMinigameScene(new MinigameModel(GameData, _estimation, item, amount));
                })
                .SelectCancel();
        }

        internal override void UpdateEstimation()
        {
            if (_materialLoaders.Any(loader => loader.Item == null))
            {
                _estimationText.text = "-";
                return;
            }
            _estimation = _estimationCurve.Evaluate((float)_materialLoaders.Average(value => value.Item.Index) / 4);
            _estimationText.text = _estimation.ToString("p2");
        }

        public void LoadPracticeGame()
        {
            Common.GameDisplay.ConfirmDialog
                .Create("You're play as PRACTICE.\nThere will be NO REWARD, but item WILL NOT LOST.\nAre you sure to play in this mode?")
                .SetTargetToResume(this)
                .SetOkAction(() =>
                {
                    LoadMinigameScene(new MinigameModel(GameData));
                })
                .SelectOk();
        }
        private void LoadMinigameScene(MinigameModel model)
        {
            Core.Global.GlobalData.Sound.PlayInScene(_startSound);
            Close(true);
            _parent.Close(true);
            MinigameManager.Init(model);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Minigame", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }
}
