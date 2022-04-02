using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    /// <summary>
    /// Loads scene. Useful for showing tip while loading.
    /// </summary>
    public static class SceneLoadingAction
    {
        public static string SceneName { get; private set; }

        public static void ChangeScene(string sceneName, bool useTip = false)
            => ChangeScene(sceneName, useTip, Color.black);
        public static void ChangeScene(string sceneName, bool useTip, Color color)
        {
            SceneName = sceneName;
            if (useTip)
            {
                ScreenFading.Create(ScreenFadingType.FadeOut, 2, color, "Tips");
                //SetTipsDisplay
            }
            else
            {
                ScreenFading.Create(ScreenFadingType.Bidirectional, 2, color, sceneName);
                //DestroyThis
            }
        }
    }
}
