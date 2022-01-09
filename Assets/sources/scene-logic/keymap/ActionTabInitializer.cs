using UnityEngine;

namespace PataRoad.SceneLogic.KeymapSettings
{
    class ActionTabInitializer : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle _firstSelect;
        public void Initialize()
        {
            _firstSelect.isOn = true;
            _firstSelect.onValueChanged.Invoke(true);
        }
    }
}
