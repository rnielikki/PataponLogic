using PataRoad.SceneLogic.Minigame;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        [Header("Preview status")]
        [SerializeField]
        private Image _previewImage;
        [SerializeField]
        private Text _previewText;
        [SerializeField]
        private Text _previewAmountText;


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
                    var (item, amount) = GetReward();
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
            var (item, amount) = GetReward();
            UpdateDescription(item, amount);
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
        public void UpdateDescription(Core.Items.IItem item, int amount)
        {
            _previewImage.sprite = item.Image;
            _previewText.text = item.Name;
            _previewAmountText.text = amount.ToString();
        }
        private (Core.Items.IItem, int) GetReward()
        {
            return _lastMinigameSelection.GetReward(
                _materialLoaders.Sum(lo => lo.Item.Index),
                _materialLoaders.Max(lo => lo.Item.Index) - _materialLoaders.Min(lo => lo.Item.Index));
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
