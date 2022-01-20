using PataRoad.Common.GameDisplay;
using PataRoad.Story.Actions;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Story
{
    class StoryLoader : MonoBehaviour
    {
        private static StoryLoader _current { get; set; }
        private UnityEngine.InputSystem.InputAction _action;
        private void Awake()
        {
            _current = this;
            _current.gameObject.SetActive(false);
            _action = Core.Global.GlobalData.Input.actions.FindAction("UI/Select");
            _action.performed += SkipStory;
        }
        public static void Init()
        {
            var gameObj = new GameObject();
            DontDestroyOnLoad(gameObj);
            _current = gameObj.AddComponent<StoryLoader>();
        }
        public static void LoadStory(StoryData data)
        {
            _current.gameObject.SetActive(true);
            _current.StartStory(data);
        }
        private void StartStory(StoryData data)
        {
            SceneLoadingAction.Create(data.SceneName).ChangeScene();
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

                var time = data.Time < 0 ? System.DateTime.Now.Hour : data.Time;
                storySceneInfo.PatapolisWeather.SetBackgroundToTime(time);
            }
            else
            {
                storySceneInfo.Background.Init(data.Background);
                storySceneInfo.Weather.Init(data.Weather);
                storySceneInfo.Wind.Init(data.Wind);
            }

            //-- Object instantiating
            var obj = Instantiate(data.gameObject, storySceneInfo.Parent);

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

            if (imageFromResource != null)
            {
                var imageFromInstance = obj.GetComponentInChildren<StoryImage>();
                imageFromResource.Init(imageFromInstance);
                imageFromInstance.Init(imageFromInstance);
            }

            //-- Do story action
            yield return storySceneInfo.LoadStoryLines(data.StoryActions);

            //-- Next or end?
            if (data.NextStory != null)
            {
                _current.StartStory(data.NextStory);
            }
            else
            {
                MoveToNext();
            }
        }
        private void SkipStory(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _action.performed -= SkipStory;
            StopAllCoroutines();
            MoveToNext();
        }
        private void MoveToNext()
        {
            //-- Move to the next map
            SceneLoadingAction.Create("Patapolis").UseTip().ChangeScene();
            //-- Story done, you don't need this anymore!
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            _current = null;
            _action.performed -= SkipStory;
        }
    }
}
