using PataRoad.Story.Actions;
using System.Collections;
using System.Linq;
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
            //-- Init
            var storySceneInfo = FindObjectOfType<StorySceneInfo>();
            storySceneInfo.StartStoryScene();

            //-- Audio
            storySceneInfo.AudioSource.clip = data.Music;
            storySceneInfo.AudioSource.Play();

            //-- Weather and other environments
            if (data.UsePatapolis)
            {
                var pos = Camera.main.transform.position;
                pos.x = data.XCameraPosition;
                Camera.main.transform.position = pos;

                storySceneInfo.Weather.ChangeWeather(data.Weather);
                storySceneInfo.Wind.StartNoWind();
                storySceneInfo.Wind.StartWind(data.Wind);
            }
            else
            {
                storySceneInfo.Background.Init(data.Background);
                storySceneInfo.Weather.Init(data.Weather);
                storySceneInfo.Wind.Init(data.Wind);
            }

            //-- Object instantiating
            var obj = Instantiate(data.gameObject, storySceneInfo.Parent);
            if (data.UsePatapolis)
            {
                var pos = obj.gameObject.transform.position;
                pos.x = data.XCameraPosition;
                obj.gameObject.transform.position = pos;
            }

            //-- Object mapping
            var fromResource = data.GetComponentsInChildren<StoryBehaviour>(true).OrderBy(s => s.Seed).ToArray();
            var fromInstance = obj.GetComponentsInChildren<StoryBehaviour>(true).OrderBy(s => s.Seed).ToArray();
            if (fromResource.Length != fromInstance.Length)
            {
                throw new MissingReferenceException("resource and instance length doesn't match.");
            }
            for (int i = 0; i < fromResource.Length; i++)
            {
                var res = fromResource[i];
                var inst = fromInstance[i];
                if (res.Seed != inst.Seed)
                {
                    throw new MissingReferenceException("resource and instance doesn't match.");
                }
                res.SetInstance(inst);
                inst.SetInstance(inst); //I don't know does really it need but in any case if they find reference on runtime...
            }
            var imageFromResource = data.GetComponentInChildren<StoryImage>();
            var imageFromInstance = obj.GetComponentInChildren<StoryImage>();

            imageFromResource.Init(imageFromInstance);
            imageFromInstance.Init(imageFromInstance);

            //-- Do story action
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

            //-- Move to the next map
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
            //-- Story done, you don't need this anymore!
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
