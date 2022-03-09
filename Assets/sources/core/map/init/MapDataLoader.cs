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
            _mapDataContainer = Global.GlobalData.CurrentSlot.MapInfo.NextMap;
            var mapData = _mapDataContainer.MapData;
            if (mapData == null)
            {
                ShowError("Map");
                return;
            }

            //-- boss.
            var boss = Global.GlobalData.CurrentSlot.PataponInfo.BossToSummon;
            if (boss != null)
            {
                _bossSummonManager.Init(boss.Index, mapData.SummonCount);
            }
            else
            {
                _bossSummonManager.InitZero();
            }

            //-- music.
            string musicName = Global.GlobalData.CurrentSlot.PataponInfo.CustomMusic == null
                ? null
                : Global.GlobalData.CurrentSlot.PataponInfo.CustomMusic.Data;
            if (musicName == null) musicName = mapData.DefaultMusic;
            _bgmPlayer.MusicTheme = musicName;

            //-- background.
            _backgroundLoader.Init(mapData.BackgroundName ?? "Ruins");

            //-- weather.
            _weatherInfo.Init(_mapDataContainer.Weather.CurrentWeather, _mapDataContainer.Weather.CurrentWind);

            //-- missionPoint.
            _missionPoint.FilledMissionCondition = mapData.FilledMissionCondition;
            _missionPoint.UseMissionTower = mapData.UseMissionTower;
            if (!mapData.LoadStoryOnlyOnce || !_mapDataContainer.Cleared)
            {
                _missionPoint.NextStory = mapData.NextStoryOnSuccess;
                _missionPoint.NextFailureStory = mapData.NextStoryOnFail;
            }
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
            if (_mapDataContainer.MapData.HasLevel)
            {
                foreach (var havingLevel in obj.GetComponentsInChildren<IHavingLevel>())
                {
                    havingLevel.SetLevel(_mapDataContainer.Level, _mapDataContainer.MapData.AbsoluteMaxLevel);
                }
            }
            int spriteIndex = 1;
            foreach (var character in obj.GetComponentsInChildren<Character.IAttackable>())
            {
                foreach (var renderer in (character as MonoBehaviour).GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.sortingOrder = spriteIndex;
                }
                spriteIndex++;
            }
        }
        private void ShowError(string missingTarget)
        {
            Common.GameDisplay.ConfirmDialog.Create($"[!] {missingTarget} data doesn't exist!")
                .HideOkButton()
                .SetCancelAction(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Patapolis"))
                .SelectCancel();
        }
    }
}
