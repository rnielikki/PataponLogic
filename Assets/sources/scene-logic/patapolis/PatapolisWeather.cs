using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    public class PatapolisWeather : MonoBehaviour
    {
        [SerializeField]
        private Core.Map.Weather.WeatherInfo _weather;
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Gradient _backgroundOverTime;
        [SerializeField]
        private Gradient _backgroundImageOverTime;
        [SerializeField]
        private UnityEngine.UI.Image _backgroundImage;
        [SerializeField]
        private GameObject _star;
        // Start is called before the first frame update
        void Awake()
        {
            var patapolisWeather = Core.Global.GlobalData.CurrentSlot.MapInfo.PatapolisWeather;
            _weather.Init(patapolisWeather.CurrentWeather);
            _weather.Wind.Init(patapolisWeather.CurrentWind);

            var currentHour = System.DateTime.Now.Hour;
            SetBackgroundToTime(currentHour);
        }
        public void SetBackgroundToTime(int currentHour)
        {
            var current = (float)Mathf.Abs(currentHour * 60 + System.DateTime.Now.Minute - 720) / 720;
            _camera.backgroundColor = _backgroundOverTime.Evaluate(current);
            _backgroundImage.color = _backgroundImageOverTime.Evaluate(current);
            if (Core.Global.GlobalData.CurrentSlot.MapInfo.PatapolisWeather.CurrentWeather == Core.Map.Weather.WeatherType.Clear
                && (currentHour < 6 || currentHour > 18))
            {
                _star.SetActive(true);
            }
        }
    }
}
