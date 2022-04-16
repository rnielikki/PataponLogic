using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// Loads scene. Useful for showing tip while loading.
    /// </summary>
    public static class SceneLoadingAction
    {
        public static string SceneName { get; private set; }

        public static void ChangeScene(string sceneName, bool useTip = false, bool loading = false)
        {
            ChangeScene(sceneName, useTip, Color.black, loading);
        }
        public static void ChangeScene(string sceneName, bool useTip, Color color, bool loading = false)
        {
            SceneName = sceneName;
            if (useTip)
            {
                ScreenFading.Create(ScreenFadingType.FadeOut, -1, color, "Tips");
                //SetTipsDisplay
            }
            else
            {
                ScreenFading.Create(ScreenFadingType.Bidirectional, -1, color, sceneName, loading);
                //DestroyThis
            }
        }
    }
}
