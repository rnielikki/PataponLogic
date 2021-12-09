using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class ItemDescriptionField : MonoBehaviour
    {
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Text _description;
        public void UpdateText(Core.Items.IItem item)
        {
            if (item == null)
            {
                _name.text = "None";
                _description.text = "Equip nothing.";
            }
            else
            {
                _name.text = item.Name;
                _description.text = item.Description;
            }
        }
    }
}