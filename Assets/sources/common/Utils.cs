using UnityEngine;

namespace PataRoad.Common
{
    public static class Utils
    {
        public static bool RandomByProbability(float probability)
        {
            float clamped = Mathf.Clamp01(probability);
            if (clamped == 1) return true;
            return Random.Range(0, 1f) < clamped;
        }
    }
}
