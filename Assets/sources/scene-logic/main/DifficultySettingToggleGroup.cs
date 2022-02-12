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
            toggle.ResetTextColor();
        }

        internal void SetCheckedToggle(DifficultySettingToggle difficultySettingToggle)
        {
            _current = difficultySettingToggle;
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

