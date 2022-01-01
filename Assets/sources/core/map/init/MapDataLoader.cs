using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map
{
    public class MapDataLoader : MonoBehaviour
    {
        [SerializeField]
        Character.Bosses.BossSummonManager _bossSummonManager;
        [SerializeField]
        Rhythm.Bgm.RhythmBgmPlayer _bgmPlayer;
        [SerializeField]
        Background.BackgroundLoader _backgroundLoader;
        [SerializeField]
        Weather.WeatherInfo _weatherInfo;
        [SerializeField]
        MissionPoint _missionPoint;
        private MapDataContainer _mapDataContainer;

        [SerializeField]
        Transform _attachTarget;
        private void Awake()
        {
            _mapDataContainer = Global.GlobalData.MapInfo.NextMap;
            var mapData = _mapDataContainer.MapData;
            if (mapData == null)
            {
                ShowError("Map");
                return;
            }

            //-- boss.
            var boss = Global.GlobalData.PataponInfo.BossToSummon;
            if (boss != null)
            {
                _bossSummonManager.Init(boss.Index, mapData.SummonCount);
            }
            else
            {
                _bossSummonManager.InitZero();
            }

            //-- music.
            string musicName = Global.GlobalData.PataponInfo.CustomMusic?.Data;
            if (musicName == null) musicName = mapData.DefaultMusic;
            _bgmPlayer.MusicTheme = musicName;

            //-- background.
            _backgroundLoader.Init(mapData?.BackgroundName ?? "Ruins");

            //-- weather.
            _weatherInfo.Init(_mapDataContainer.Weather.CurrentWeather);
            _weatherInfo.Wind.Init(_mapDataContainer.Weather.CurrentWind);

            //-- missionPoint.
            _missionPoint.FilledMissionCondition = mapData.FilledMissionCondition;
            _missionPoint.UseMissionTower = mapData.UseMissionTower;
            _missionPoint.NextStory = mapData.NextStoryOnSuccess;
            _missionPoint.NextFailureStory = mapData.NextStoryOnFail;
        }
        private void Start()
        {
            //-- Finally Game Object.
            GameObject asset = Resources.Load<GameObject>($"{Global.MapInfo.MapPath}GameObjects/{_mapDataContainer.MapData.Index}");
            if (asset == null)
            {
                ShowError("Object");
                return;
            }
            var obj = Instantiate(asset, _attachTarget);
            foreach (var havingLevel in obj.GetComponents<MonoBehaviour>().OfType<IHavingLevel>())
            {
                havingLevel.SetLevel(_mapDataContainer.Level);
            }
        }
        private void ShowError(string missingTarget)
        {
            Common.GameDisplay.ConfirmDialog.CreateCancelOnly($"[!] {missingTarget} data doesn't exist!", onCanceled: () => UnityEngine.SceneManagement.SceneManager.LoadScene("Patapolis"));
        }
    }
}
