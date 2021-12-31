using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    class GuideSpecialKeyUpdater : MonoBehaviour
    {
        [SerializeField]
        private string _bindingName;
        [SerializeField]
        private string _subBindingName;
        [SerializeField]
        private UnityEngine.UI.Text _output;
        private void Start()
        {
            if (!Core.Global.GlobalData.GlobalInputActions.TryGetAllActionBindingNames(_bindingName, out var dict))
            {
                gameObject.SetActive(false);
            }
            else if (!dict.TryGetValue(_subBindingName, out string name))
            {
                if (Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName(_bindingName, out string fullName))
                {
                    _output.text = fullName;
                }
                else gameObject.SetActive(false);
            }
            else _output.text = name;
        }
    }
}
