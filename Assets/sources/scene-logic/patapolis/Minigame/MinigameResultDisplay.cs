using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Minigame
{
    internal class MinigameResultDisplay : MonoBehaviour
    {
        [SerializeField]
        Text _result;
        [SerializeField]
        Text _requirement;
        [SerializeField]
        GameObject _rewardGroup;
        [SerializeField]
        Text _itemName;
        [SerializeField]
        Image _itemImage;
        [SerializeField]
        Text _itemAmountField;
        [SerializeField]
        Button _closeButton;
        [SerializeField]
        AudioClip _onSuccess;
        [SerializeField]
        AudioClip _onFail;

        private UnityEngine.Events.UnityEvent _onMinigameClosed;

        public void UpdateResult(MinigameModel model, float result, UnityEngine.Events.UnityEvent onMinigameClosed)
        {
            gameObject.SetActive(true);
            _onMinigameClosed = onMinigameClosed;
            _result.text = result.ToString("p0");

            if (model.IsPractice)
            {
                _rewardGroup.SetActive(false);
                _requirement.text = "";
            }
            else UpdateRealResult(model, result >= model.ClearRequirement);
            _closeButton.Select();
        }
        private void UpdateRealResult(MinigameModel model, bool success)
        {
            _requirement.text = model.ClearRequirement.ToString("p0");
            if (!success)
            {
                _rewardGroup.SetActive(false);
                Core.Global.GlobalData.Sound.PlayGlobal(_onFail);
            }
            else
            {
                _itemName.text = model.Reward.Name;
                _itemImage.sprite = model.Reward.Image;
                _itemAmountField.text = "x " + model.RewardAmount.ToString();
                Core.Global.GlobalData.Inventory.AddMultiple(model.Reward, model.RewardAmount);
                Core.Global.GlobalData.Sound.PlayGlobal(_onSuccess);
            }
        }
        public void Close()
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Minigame");
            _onMinigameClosed.Invoke();
            Core.Global.GlobalData.MapInfo.RefreshAllWeathers();
        }
    }
}
