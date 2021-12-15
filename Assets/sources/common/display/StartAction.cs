using UnityEngine;

namespace PataRoad.Common
{
    public class StartAction : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent _onStarted;
        private void Start()
        {
            _onStarted.Invoke();
            _onStarted.RemoveAllListeners();
        }
        private void OnDestroy()
        {
            _onStarted.RemoveAllListeners();
        }
    }
}
