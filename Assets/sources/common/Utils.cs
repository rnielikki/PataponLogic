using UnityEngine;

namespace PataRoad.Common
{
    public static class Utils
    {
        public static bool RandomByProbability(float probability) => Random.Range(0, 1f) < Mathf.Clamp01(probability);
    }
}
