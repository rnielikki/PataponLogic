using UnityEngine;

namespace PataRoad.Common
{
    /// <summary>
    /// Simple transform move, rotate, scale wrapper for events. Only for 2D.
    /// </summary>
    class TransformChanger : MonoBehaviour
    {
        public void MoveX(float offset) => transform.Translate(offset, 0, 0);
        public void MoveY(float offset) => transform.Translate(0, offset, 0);
        public void Rotate(float angleAsDegree) => transform.Rotate(0, 0, angleAsDegree);
        public void ScaleX(float scale)
        {
            var localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;
        }
        public void ScaleY(float scale)
        {
            var localScale = transform.localScale;
            localScale.y = scale;
            transform.localScale = localScale;
        }
    }
}
