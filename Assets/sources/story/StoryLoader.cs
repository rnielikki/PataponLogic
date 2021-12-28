using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Story
{
    class StoryLoader : MonoBehaviour
    {
        [SerializeField]
        private StoryLineDisplay _display;
        private static StoryLoader _current { get; set; }
        private void Start()
        {
            _current = this;
            _current.gameObject.SetActive(false);
        }
        public static void Init()
        {
            _current.gameObject.SetActive(true);
            DontDestroyOnLoad(_current);
        }
        public static void LoadStory(StoryData data)
        {
            _current.StartStory(data);
        }
        private void StartStory(StoryData data)
        {
            Common.SceneLoadingAction.Create(data.SceneName, false);
            SceneManager.sceneLoaded += OnStorySceneLoaded;
            void OnStorySceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= OnStorySceneLoaded;
                _current.StartCoroutine(ReadStory(data));
            }
        }
        private IEnumerator ReadStory(StoryData data)
        {
            var storySceneInfo = FindObjectOfType<StorySceneInfo>();

            storySceneInfo.Weather.Init(data.Weather);
            storySceneInfo.Wind.Init(data.Wind);

            storySceneInfo.AudioSource.clip = data.Music;
            storySceneInfo.AudioSource.Play();


            if (data.UsePatapolis)
            {
                var pos = Camera.main.transform.position;
                pos.x = data.XCameraPosition;
                pos.y = 5;
                Camera.main.transform.position = pos;
                Camera.main.orthographicSize = 10;
            }
            else
            {
                storySceneInfo.Background.Init(data.Background);
            }
            Instantiate(data.Resource, storySceneInfo.Parent);
            foreach (var storyAction in data.StoryActions)
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
                if (waitingForLine != null) yield return waitingForLine;
            }

            if (data.NextMap != null)
            {
                var mapContainer = Core.Global.GlobalData.MapInfo.GetMapByIndex(data.NextMap.Index);
                if (mapContainer != null)
                {
                    Core.Global.GlobalData.MapInfo.Select(mapContainer);
                    Common.SceneLoadingAction.Create("Battle", false);
                }
                else
                {
                    Common.SceneLoadingAction.Create("Patapolis", true);
                }
            }
            else
            {
                Common.SceneLoadingAction.Create("Patapolis", true);
            }
            Destroy(gameObject);

            IEnumerator WaitForSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
            }
        }
        private void OnDestroy()
        {
            _current = null;
        }
    }
}
