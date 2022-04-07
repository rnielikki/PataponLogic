using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Core.Character.Equipments
{
    /// <summary>
    /// Returns attached object back to ObjectPool
    /// </summary>
    public class ReleaseToPool : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool;
        /// <summary>
        /// Releases "this" object to pool (the one which has this script).
        /// </summary>
        public void ReleaseThisObject()
        {
            if (Pool != null)
            {
                Pool.Release(gameObject);
            }
        }
    }
}
