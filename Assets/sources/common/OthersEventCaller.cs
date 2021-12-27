using UnityEngine;

namespace PataRoad.Common
{
    /// <summary>
    /// For Animation purpose, this calls another ones like parent's  method.
    /// </summary>
    class OthersEventCaller : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent _events;
        public void Call() => _events.Invoke();
    }
}
