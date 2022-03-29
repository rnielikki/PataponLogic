using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.ItemExchange
{
    public class ExchangeSelectionUpdater : MonoBehaviour
    {
        [SerializeField]
        private Image _previewImage;
        [SerializeField]
        private Text _previewText;
        [SerializeField]
        private Text _previewAmountText;
        [SerializeField]
        Text _requirementText;
        [SerializeField]
        Text _rateText;
        private void Start()
        {
            Hide();
        }
        public void UpdateDescription(ExchangeMaterialSelection selection)
        {
            gameObject.SetActive(true);
            _previewImage.sprite = selection.OutputItem.Image;
            _previewText.text = selection.OutputItem.Name;
            _previewAmountText.text =
                Core.Global.GlobalData.CurrentSlot.Inventory.GetAmount(selection.OutputItem).ToString();
            _requirementText.text = selection.AmountRequirement.ToString();
            _rateText.text = selection.AmountRatio.ToString("P2");
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}