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
        private MapData _mapData;

        [SerializeField]
        Transform _attachTarget;
        private void Awake()
        {
            var nextMap = Global.GlobalData.MapInfo.NextMap;
            _mapData = nextMap.MapData;
            if (_mapData == null)
            {
                ShowError("Map");
                return;
            }

            //-- boss.
            var boss = Global.GlobalData.PataponInfo.BossToSummon;
            if (boss != null)
            {
                _bossSummonManager.Init(boss.Index, _mapData.SummonCount);
            }
            else
            {
                _bossSummonManager.InitZero();
            }

            //-- music.
            string musicName = Global.GlobalData.PataponInfo.CustomMusic?.Data;
            if (musicName == null) musicName = _mapData.DefaultMusic;
            _bgmPlayer.MusicTheme = musicName;

            //-- background.
            _backgroundLoader.Init(_mapData?.BackgroundName ?? "Ruins");

            //-- weather.
            _weatherInfo.Init(nextMap.CurrentWeather);
            _weatherInfo.Wind.Init(nextMap.CurrentWind);

            //-- missionPoint.
            _missionPoint.FilledMissionCondition = _mapData.FilledMissionCondition;
            _missionPoint.UseMissionTower = _mapData.UseMissionTower;
        }
        private void Start()
        {
            //-- Finally Game Object.
            GameObject asset = Resources.Load<GameObject>($"{Global.MapInfo.MapPath}GameObjects/{_mapData.Index}");
            if (asset == null)
            {
                ShowError("Object");
                return;
            }
            Instantiate(asset, _attachTarget);
        }
        private void ShowError(string missingTarget)
        {
            Common.GameDisplay.ConfirmDialog.CreateCancelOnly($"[!] {missingTarget} data doesn't exist!", () => UnityEngine.SceneManagement.SceneManager.LoadScene("Patapolis"));
        }
    }
}
