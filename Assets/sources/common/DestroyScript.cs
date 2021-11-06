using UnityEngine;

namespace PataRoad.Common
{
    /// <summary>
    /// Any simple destroy script to attach. For example, this can be called end of animation as Animation Event.
    /// </summary>
    public class DestroyScript : MonoBehaviour
    {
        /// <summary>
        /// Destroys "this" object (the one which has this script).
        /// </summary>
        /// <note>Avoided too common name "Destroy" for animation event.</note>
        public void DestroyThisObject() => Destroy(gameObject);
    }
}
