using PataRoad.Common.GameDisplay;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PataRoad.Story
{
    class StoryLoader : MonoBehaviour
    {
        private static StoryLoader _current { get; set; }
        private UnityEngine.InputSystem.InputAction _action;
        private bool _skipActionAdded;
        private bool _noDestroy;
        private const string _storyPath = "Story/Data/";

        private void Awake()
        {
            _current = this;
            _current.gameObject.SetActive(false);
            _action = Core.Global.GlobalData.Input.actions.FindAction("UI/Select");
        }
        public static void Init()
        {
            var gameObj = new GameObject();
            DontDestroyOnLoad(gameObj);
            _current = gameObj.AddComponent<StoryLoader>();
        }
        public static void LoadStory(string dataName)
        {
            _current.gameObject.SetActive(true);
            var data = Resources.Load<StoryData>(_storyPath + dataName);
            if (data == null)
            {
                throw new MissingReferenceException("The story data " + dataName + " doesn't exist!");
            }
            _current.StartStory(data, Color.black);
        }
        internal void StartStory(StoryData data, Color color)
        {
            _noDestroy = true;
            SceneLoadingAction.ChangeScene(data.SceneName, false, color);
            SceneManager.sceneLoaded += OnStorySceneLoaded;
            void OnStorySceneLoaded(Scene scene, LoadSceneMode mode)
            {
                _noDestroy = false;
                SceneManager.sceneLoaded -= OnStorySceneLoaded;
                _current.StartCoroutine(ReadStory(data));
            }
        }
        private IEnumerator ReadStory(StoryData rawData)
        {
            //-- Init
            var storySceneInfo = FindObjectOfType<StorySceneInfo>();
            var data = Instantiate(rawData, storySceneInfo.Parent);
            storySceneInfo.StartStoryScene();
            data.InvokeBeforeStart();
            if (!_skipActionAdded)
            {
                _current._action.performed += _current.SkipStory;
                _skipActionAdded = true;
            }

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
                storySceneInfo.Wind.StartWind(data.Wind);

                var time = data.Time < 0 ? System.DateTime.Now.Hour : data.Time;
                storySceneInfo.PatapolisWeather.SetBackgroundToTime(time);
            }
            else
            {
                storySceneInfo.Background.Init(data.Background);
                storySceneInfo.Weather.Init(data.Weather, data.Wind);
            }

            //-- Do story action
            yield return storySceneInfo.LoadStoryLines(data.StoryActions, data.ChoiceSelector, data.NextStory);
            if (!_noDestroy) MoveToNext();
            else yield return new WaitUntil(() => !_noDestroy);
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
            SceneLoadingAction.ChangeScene("Patapolis", true);
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
