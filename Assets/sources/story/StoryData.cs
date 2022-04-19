using UnityEngine;

namespace PataRoad.Story
{
    public class StoryData : MonoBehaviour
    {
        [Header("Weather info")]
        [SerializeField]
        private Core.Map.Weather.WeatherType _weather;
        [SerializeField]
        private bool _randomWeather;
        public Core.Map.Weather.WeatherType Weather => _randomWeather ? GetRandomWeather() : _weather;
        [SerializeField]
        private bool _randomWind;
        [SerializeField]
        private Core.Map.Weather.WindType _windType;
        public Core.Map.Weather.WindType Wind => _randomWind ? GetRandomWind() : _windType;
        [Header("Scene Info")]
        [SerializeField]
        private bool _usePatapolis;
        public bool UsePatapolis => _usePatapolis;
        public string SceneName => _usePatapolis ? "Patapolis" : "StoryScene";
        // Note here. NextMap is removed because "reading story before start" is annoying.
        // Especially when Mission fails too many times.
        // Story must be shown AFTER the Mission.
        [Header("Map info")]
        [SerializeField]
        [Tooltip("This do nothing on Patapolis")]
        private string _background;
        public string Background => _background;
        [SerializeField]
        private AudioClip _music;
        public AudioClip Music => _music;
        [SerializeField]
        [Tooltip("This works only on Patapolis")]
        private float _xCameraPosition;
        public float XCameraPosition => _xCameraPosition;
        [SerializeField]
        [Tooltip("This also works only on Patapolis, if time is less than zero, it'll show current time.")]
        private int _time = -1;
        public int Time => _time;
        [Header("Story Data")]
#pragma warning disable S1104 // Fields should not have public accessibility - we need AGAIN this for serialized array in inspector.
        public StoryAction[] StoryActions;
#pragma warning restore S1104 // Fields should not have public accessibility
        [SerializeField]
        ChoiceSelector _choiceSelector;
        internal ChoiceSelector ChoiceSelector => _choiceSelector;
        [SerializeField]
        StoryData _nextStory;
        internal StoryData NextStory => _nextStory;
        [SerializeField]
        Color _sceneChangingColor = Color.black;
        public Color SceneChangingColor => _sceneChangingColor;
        [SerializeField]
        [Header("This is called before skipping is possible, which makes sure to be called at least once")]
        UnityEngine.Events.UnityEvent _onStoryStarted;

        private Core.Map.Weather.WeatherType GetRandomWeather()
        {
            var weathers = System.Enum.GetValues(typeof(Core.Map.Weather.WeatherType));
            return (Core.Map.Weather.WeatherType)Random.Range(0, weathers.Length);
        }
        private Core.Map.Weather.WindType GetRandomWind()
        {
            var winds = System.Enum.GetValues(typeof(Core.Map.Weather.WindType));
            return (Core.Map.Weather.WindType)Random.Range(0, winds.Length);
        }
        internal void InvokeBeforeStart() => _onStoryStarted?.Invoke();
        public void SkipNextStory() => _nextStory = null;

        //IMAGE VALIDATION SCRIPT
        //WHENEVER YOU NEED CHECK VALIDATION REMOVE THIS COMMENT
        /*
        private void OnValidate()
        {
            foreach (var st in StoryActions)
            {
                if (!st.UseLine || st.Image == null || st.Name.IndexOf(",") >= 0
                    || st.Name == "???") continue;
                var name = st.Name.ToLower();
                var imgName = st.Image.name;
                if (imgName.StartsWith("pata"))
                {
                    return;
                }
                else if (imgName != "may")
                {
                    var index = st.Image.name.IndexOf('.');
                    if (index >= 0)
                    {
                        imgName = imgName.Substring(0, index).ToLower();
                    }
                    switch (imgName)
                    {
                        case "rahgashapon":
                            imgName = "rah";
                            break;
                        case "suko":
                            imgName = "sukopon";
                            break;
                        case "tonkampon":
                            imgName = "ton";
                            break;
                    }
                }
                var index1 = name.IndexOf(" ");
                var index2 = name.IndexOf("-");
                if (index1 != -1) name = name.Substring(0, index1);
                else if (index2 != -1) name = name.Substring(0, index2);
                if (name != imgName)
                {
                    Debug.Log("Unmatched sprite in index" + System.Array.IndexOf(StoryActions, st)
                        + ", Check " + name + " and " + imgName + " from " + transform.root.name, gameObject);
                }
            }
        }
        */
    }
}
