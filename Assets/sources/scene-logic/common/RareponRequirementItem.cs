using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.CommonSceneLogic
{
    class RareponRequirementItem : MonoBehaviour
    {
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Image _image;
        public bool SetValues(Core.Items.ItemRequirement requirement, Color availableColor, Color notAvailableColor)
        {
            gameObject.SetActive(true);
            _image.sprite = requirement.Item.Image;
            var amount = Core.Global.GlobalData.CurrentSlot.Inventory.GetAmount(requirement.Item);
            _text.text = $"{requirement.Item.Name} ({amount}/{requirement.Amount})\n";
            if (amount < requirement.Amount)
            {
                _text.color = notAvailableColor;
                return false;
            }
            else
            {
                _text.color = availableColor;
                return true;
            }
        }
        public void Hide() => gameObject.SetActive(false);
    }
}
