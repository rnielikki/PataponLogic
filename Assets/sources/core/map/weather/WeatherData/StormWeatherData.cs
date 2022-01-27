using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    class StormWeatherData : MonoBehaviour, IWeatherData
    {
        [SerializeField]
        RainWeatherData _rainData;
        public WeatherType Type => WeatherType.Storm;
        [SerializeField]
        public float _interval;

        [SerializeField]
        private AudioClip _thunderSound;
        [SerializeField]
        private AudioClip _lightningSound;
        private GameObject _lightning;

        private void Start()
        {
            _lightning = GetComponentInChildren<LightningContact>(true).gameObject;
            _lightning.transform.parent = transform.root.parent;
            _lightning.transform.position = WeatherInfo.Current.gameObject.transform.position;
        }
        public void OnWeatherStarted()
        {
            _rainData.OnWeatherStarted();
            WeatherInfo.Current.Wind.StartWind(WindType.TailWind);
            StartCoroutine(DropThunder());
        }

        public void OnWeatherStopped(WeatherType newType)
        {
            WeatherInfo.Current.Wind.StopWind(WindType.TailWind);
            StopAllCoroutines();
            if (newType != WeatherType.Rain)
            {
                _rainData.OnWeatherStopped(newType);
            }
        }
        System.Collections.IEnumerator DropThunder()
        {
            while (true)
            {
                yield return new WaitForSeconds(_interval);
                if (Common.Utils.RandomByProbability(0.5f))
                {
                    WeatherInfo.Current.AudioSource.PlayOneShot(_lightningSound);
                    _lightning.SetActive(true);
                    _lightning.transform.position = new Vector2(
                        Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0, 1f), 0.5f)).x,
                        _lightning.transform.position.y);
                    yield return new WaitForSeconds(1);
                    _lightning.SetActive(false);
                }
                else
                {
                    WeatherInfo.Current.AudioSource.PlayOneShot(_thunderSound);
                }
            }
        }
    }
}
