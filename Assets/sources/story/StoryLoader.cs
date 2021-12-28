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


            if (data.UsePatapolis)
            {
                var pos = Camera.main.transform.position;
                pos.x = data.XCameraPosition;
                Camera.main.transform.position = pos;
            }
            else
            {
                storySceneInfo.Background.Init(data.Background);
            }
            Instantiate(data.Resource, storySceneInfo.Parent);
            foreach (var storyAction in data.StoryActions)
            {
                storyAction.InvokeEvent();
                if (storyAction.UseLine)
                {
                    yield return _display.WaitUntilNext(storyAction);
                }
            }

            yield return null;
            //Destory(gameObject)
        }
        private void OnDestroy()
        {
            _current = null;
        }
    }
}
