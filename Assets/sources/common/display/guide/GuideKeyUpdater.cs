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
        [SerializeField]
        bool _isComposite;
        private void Start()
        {
            bool gotBinding = false;
            if (_isComposite)
            {
                gotBinding = Core.Global.GlobalData.GlobalInputActions.TryGetAllActionBindingNames(_bindingName, out var dictionary);
                if (!gotBinding)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _output.text = string.Join('/', dictionary.Values);
            }
            else
            {
                gotBinding = Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName(_bindingName, out string name);
                if (!gotBinding)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _output.text = name;
            }
            if (_useDescription) _output.text += " : " + _description;
        }
    }
}
