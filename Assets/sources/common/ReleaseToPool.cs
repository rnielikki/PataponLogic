using PataRoad.Core.Character.Equipments.Logic;
using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Common
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
        public void ReleaseThisObject() => Pool.Release(gameObject);
    }
}
