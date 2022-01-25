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
        /// <summary>
        /// Loads story lines without need to load whole environment. Also <see cref="StoryLoader"/> uses this.
        /// </summary>
        /// <param name="stories">The array of story action data sequence.</param>
        /// <param name="choiceSelector">Choice window after stories are performed. Let as <c>null</c> if no choice will be shown.</param>
        /// <param name="nextStory">Next story after stories are performed. Let as <c>null</c> if no next story to load. DOESN'T check recursion.</param>
        /// <returns>Yield that waits until the input (or seconds).</returns>
        public IEnumerator LoadStoryLines(StoryAction[] stories, ChoiceSelector choiceSelector, StoryData nextStory)
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
            _display.Close();
            if (choiceSelector != null)
            {
                yield return choiceSelector.Open(this);
            }
            else if (nextStory != null)
            {
                _display.Close();
                var storyLoader = FindObjectOfType<StoryLoader>();
                storyLoader.StartStory(nextStory);
            }

            IEnumerator WaitForSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
            }
        }

    }
}
