using UnityEngine;

namespace PataRoad.Common
{
    public static class Utils
    {
        public static bool RandomByProbability(float probability) => Random.Range(0, 1) < Mathf.Clamp01(probability);
    }
}
