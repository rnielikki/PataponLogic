using UnityEngine;

namespace PataRoad.Common
{
    class AutoStart : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent _events;
        private void Start()
        {
            _events.Invoke();
        }

        private void OnDestroy()
        {
            _events.RemoveAllListeners();
        }
    }
}
