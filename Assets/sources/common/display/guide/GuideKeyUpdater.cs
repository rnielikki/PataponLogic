using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    class GuideKeyUpdater : MonoBehaviour
    {
        [SerializeField]
        private string _bindingName;
        [SerializeField]
        private bool _useDescription;
        [SerializeField]
        private string _description;
        [SerializeField]
        private UnityEngine.UI.Text _output;
        private void Start()
        {
            if (!Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName(_bindingName, out string name))
            {
                gameObject.SetActive(false);
            }
            else _output.text = name;
            if (_useDescription) _output.text += " : " + _description;
        }
    }
}
