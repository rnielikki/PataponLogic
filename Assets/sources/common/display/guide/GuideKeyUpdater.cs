using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    class GuideKeyUpdater : MonoBehaviour
    {
        [SerializeField]
        private string _bindingName;
        [SerializeField]
        private UnityEngine.UI.Text _output;
        private void Start()
        {
            if (!Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName(_bindingName, out string name))
            {
                gameObject.SetActive(false);
            }
            else _output.text = name;
        }
    }
}
