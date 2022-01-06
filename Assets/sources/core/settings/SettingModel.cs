﻿using UnityEngine;

namespace PataRoad.Core.Global.Settings
{
    [System.Serializable]
    public class SettingModel
    {
        [SerializeField]
        Rhythm.Difficulty _difficulty;
        public Rhythm.Difficulty Difficulty => _difficulty;
        [SerializeField]
        float _musicVolume;
        public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
        [SerializeField]
        float _soundVolume;
        public float SoundVolume { get => _soundVolume; set => _soundVolume = value; }

        /// <summary>
        /// Loads default values. For loading existing values, Use <see cref="Load"/> instead.
        /// </summary>
        private SettingModel Init()
        {
            SetDifficulty(Rhythm.Difficulty.Normal);
            MusicVolume = 0.75f;
            SoundVolume = 1;
            return this;
        }
        internal static SettingModel Load()
        {
            var jsonData = PlayerPrefs.GetString("Settings");
            if (string.IsNullOrEmpty(jsonData))
            {
                return new SettingModel().Init();
            }
            else
            {
                return JsonUtility.FromJson<SettingModel>(jsonData);
            }
        }
        public void UpdateData(SettingModel model)
        {
            if (model.Difficulty != Difficulty)
            {
                SetDifficulty(model.Difficulty);
            }
            MusicVolume = model.MusicVolume;
            SoundVolume = model.SoundVolume;
            Save();
        }
        public void SetDifficulty(Rhythm.Difficulty difficulty)
        {
            Rhythm.RhythmEnvironment.ChangeDifficulty(difficulty);
            _difficulty = difficulty;
        }
        //will change later w/ input settings!!
        public SettingModel Copy() => (SettingModel)MemberwiseClone();

        private void Save()
        {
            PlayerPrefs.SetString("Settings", JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }
    }
}