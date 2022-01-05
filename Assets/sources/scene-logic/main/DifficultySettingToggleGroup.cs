using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Main.SettingScene
{
    class DifficultySettingToggleGroup : MonoBehaviour
    {
        DifficultySettingToggle _easy;
        DifficultySettingToggle _normal;
        DifficultySettingToggle _hard;

        DifficultySettingToggle _current;

        [SerializeField]
        Selectable[] _elementsOnUp;
        [SerializeField]
        Selectable[] _elementsOnDown;

        private void Awake()
        {
            foreach (var toggle in GetComponentsInChildren<DifficultySettingToggle>())
            {
                switch (toggle.Difficulty)
                {
                    case Core.Rhythm.Difficulty.Easy:
                        _easy = toggle;
                        break;
                    case Core.Rhythm.Difficulty.Normal:
                        _normal = toggle;
                        break;
                    case Core.Rhythm.Difficulty.Hard:
                        _hard = toggle;
                        break;
                }
            }
        }
        internal void Init(Core.Global.Settings.SettingModel setting)
        {
            var toggle = GetToggle(setting.Difficulty);
            GetToggle(setting.Difficulty).SwitchOn(true);
            SetCheckedToggle(toggle);
        }

        internal void SetCheckedToggle(DifficultySettingToggle difficultySettingToggle)
        {
            _current = difficultySettingToggle;
            foreach (var selectable in _elementsOnUp)
            {
                var nav = selectable.navigation;
                nav.selectOnDown = _current.Selectable;
                selectable.navigation = nav;
            }
            foreach (var selectable in _elementsOnDown)
            {
                var nav = selectable.navigation;
                nav.selectOnUp = _current.Selectable;
                selectable.navigation = nav;
            }
        }

        private DifficultySettingToggle GetToggle(Core.Rhythm.Difficulty difficulty) =>
            difficulty switch
            {
                Core.Rhythm.Difficulty.Easy => _easy,
                Core.Rhythm.Difficulty.Normal => _normal,
                Core.Rhythm.Difficulty.Hard => _hard,
                _ => throw new System.NotImplementedException()
            };

        internal Core.Rhythm.Difficulty GetValueFromToggle() => _current.Difficulty;
    }
}

