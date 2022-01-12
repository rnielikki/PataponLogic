using UnityEngine;
using TMPro;
using PataRoad.Common.GameDisplay;
using PataRoad.Core.Rhythm;

namespace PataRoad.SceneLogic.Intro
{
    class TutorialPonDisplay : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _text;
        [SerializeField]
        TutorialDrumUpdater _drumUpdater;
        [SerializeField]
        TextMeshProUGUI _singingText;
        int _singCount;

        [SerializeField]
        AudioSource _soundSource;
        [SerializeField]
        AudioClip _failSound;

        private string _ponDrumName;

        internal void LoadKeyName()
        {
            if (!Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName("Drum/Pon", out _ponDrumName))
            {
                ConfirmDialog.Create("No binding for PON drum found. The game may not work correctly.")
                    .HideOkButton()
                    .SelectCancel();
            }
        }
        internal void UpdateText(string text)
        {
            _text.text = string.Format(text, _ponDrumName);
        }

        internal TutorialDrumUpdater StartDrumTutorial(int practiceCount)
        {
            var drums = new DrumType[practiceCount];
            System.Array.Fill(drums, DrumType.Pon);
            _drumUpdater.LoadDrums(drums);
            return _drumUpdater;
        }
        internal void StartTurnTutorial()
        {
            foreach (Transform child in _drumUpdater.transform) Destroy(child.gameObject);
            _singingText.gameObject.SetActive(true);
            _drumUpdater.LoadDrums(
                    new DrumType[]
                    {
                        DrumType.Pon,
                        DrumType.Pon,
                        DrumType.Pon,
                        DrumType.Pon
                    }
                );
        }
        internal void SingOnTurn()
        {
            _singCount = 0;
            _soundSource.Play();
            _drumUpdater.ResetHit();
            Sing();
            RhythmTimer.Current.OnTime.AddListener(Sing);
        }
        internal void StopSinging(bool isFailed)
        {
            _singCount = 0;
            if (isFailed)
            {
                _soundSource.Stop();
                PlayFailedSound();
            }
            RhythmTimer.Current.OnTime.RemoveListener(Sing);
            _singingText.text = "-";
        }
        internal void PlayFailedSound() => _soundSource.PlayOneShot(_failSound);
        private void Sing()
        {
            _singingText.text += " PON ";
            _singCount++;
            if (_singCount > 4)
            {
                RhythmTimer.Current.OnTime.RemoveListener(Sing);
                StopSinging(false);
            }
        }
    }
}
