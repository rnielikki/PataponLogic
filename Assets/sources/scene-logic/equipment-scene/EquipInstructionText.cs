namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipInstructionText : UnityEngine.MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var textElement = GetComponent<UnityEngine.UI.Text>();
            if (Core.Global.GlobalData.TryGetActionBindingName("UI/Equip", out string name))
            {
                textElement.text = $"{name} to optimize";
            }
            else
            {
                textElement.text = "No bounding found for optimizing";
            }
        }
    }
}
