using PataRoad.Core.Map.Weather;
using System.Collections;
using UnityEngine;

namespace PataRoad.Story
{
    class StorySceneInfo : MonoBehaviour
    {
        [SerializeField]
        WeatherInfo _weatherInfo;
        internal WeatherInfo Weather => _weatherInfo;
        [SerializeField]
        Wind _wind;
        internal Wind Wind => _wind;
        [SerializeField]
        AudioSource _audioSource;
        internal AudioSource AudioSource => _audioSource;
        [SerializeField]
        private SceneLogic.Patapolis.PatapolisWeather _patapolisWeather;
        public SceneLogic.Patapolis.PatapolisWeather PatapolisWeather => _patapolisWeather;
        [SerializeField]
        Core.Map.Background.BackgroundLoader _background;
        internal Core.Map.Background.BackgroundLoader Background => _background;
        [SerializeField]
        Transform _parent;
        [SerializeField]
        UnityEngine.Events.UnityEvent _whenUseSceneForStory;
        [SerializeField]
        private StoryLineDisplay _display;
        [SerializeField]
        private AudioClip _nextSound;
        public void StartStoryScene() => _whenUseSceneForStory.Invoke();
        internal Transform Parent => _parent;
        public IEnumerator LoadStoryLines(StoryAction[] stories)
        {
            foreach (var storyAction in stories)
            {
                storyAction.InvokeEvent();
                Coroutine waitingForSeconds = null;
                Coroutine waitingForLine = null;
                if (storyAction.WaitingSeconds > 0)
                {
                    waitingForSeconds = StartCoroutine(WaitForSeconds(storyAction.WaitingSeconds));
                }
                if (storyAction.UseLine)
                {
                    waitingForLine = StartCoroutine(_display.WaitUntilNext(storyAction));
                }
                else
                {
                    _display.Close();
                }
                if (waitingForSeconds != null) yield return waitingForSeconds;
                if (waitingForLine != null)
                {
                    yield return waitingForLine;
                    _audioSource.PlayOneShot(_nextSound);
                }
            }
            IEnumerator WaitForSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
            }
        }

    }
}
