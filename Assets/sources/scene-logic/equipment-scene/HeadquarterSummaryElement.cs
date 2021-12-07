using System.Linq;
using UnityEngine.InputSystem;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class HeadquarterSummaryElement : SummaryElement
    {
        [UnityEngine.SerializeField]
        UnityEngine.Events.UnityEvent _onSubmit;
        public UnityEngine.Events.UnityEvent OnSubmit => _onSubmit;

        [UnityEngine.SerializeField]
        string _bindingName;
        private void Awake()
        {
            if (!string.IsNullOrEmpty(_bindingName) && Core.GlobalData.TryGetActionBindingName(_bindingName, out string result))
            {
                GetComponentInChildren<UnityEngine.UI.Text>().text += $" ({result})";
            }
        }
        private void OnDestroy()
        {
            _onSubmit?.RemoveAllListeners();
        }
    }
}
