using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;
using TMPro;
using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    public class SongTutorial : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _instruction;
        [SerializeField]
        private TutorialDrumUpdater _command;
        [SerializeField]
        private TextMeshProUGUI _bottomText;
        private Core.Items.SongItemDropData _data;
        // Start is called before the first frame update
        private RhythmCommand _rhythmCommand;
        private PracticingCommandListData _commandListData;
        private bool _speaking;
        void Awake()
        {
            _bottomText.enabled = false;
            _rhythmCommand = FindObjectOfType<RhythmCommand>();
        }
        public void Init(CommandSong song, Core.Items.SongItemDropData data)
        {
            _data = data;
            StartCoroutine(ShowInstruction(song));
        }

        System.Collections.IEnumerator ShowInstruction(CommandSong song)
        {
            _rhythmCommand.enabled = false;
            _instruction.text = _data.StartMessage;
            yield return new WaitForSeconds(4);
            _rhythmCommand.enabled = true;
            _bottomText.enabled = true;

            _commandListData = _rhythmCommand.ToPracticeMode(song);
            _command.Load(_commandListData);

            _commandListData.OnHit.AddListener(UpdateInstruction);
            _commandListData.OnCommand.AddListener(UpdateInstructionOnCommand);
            _commandListData.OnPracticeEnd.AddListener(EndInstruction);
            _rhythmCommand.OnCommandCanceled.AddListener(UpdateInstructionOnMiss);

            _instruction.text = _data.ProcessingMessage[0];
            SpeakTeaching();
        }
        private void UpdateInstruction(System.Collections.Generic.IEnumerable<DrumType> drums, int count)
        {
            _command.PlayOnIndex(count - 1);
        }
        private void UpdateInstructionOnCommand(System.Collections.Generic.IEnumerable<DrumType> drums, int count)
        {
            _instruction.text = _data.ProcessingMessage[count];
            _instruction.color = Color.black;
            _command.ResetHit();
        }

        private void UpdateInstructionOnMiss()
        {
            if (!_speaking)
            {
                SpeakTeaching();
            }
            _instruction.text = "Keep calm and press";
            _instruction.color = Color.red;
            _command.ResetHit();
        }
        public void EndInstruction()
        {
            StopAllCoroutines();
            RemoveListeners();
            _instruction.color = Color.black;
            _instruction.text = _data.EndMessage;
            _command.gameObject.SetActive(false);
            _bottomText.enabled = false;
            _data.EndPractice();
            StartCoroutine(EndInstructionOnTime());
            System.Collections.IEnumerator EndInstructionOnTime()
            {
                yield return new WaitForSeconds(8);
                Destroy(gameObject);
            }
        }
        private void RemoveListeners()
        {
            StopAllCoroutines();
            _commandListData?.OnHit.RemoveAllListeners();
            _commandListData?.OnCommand.RemoveAllListeners();
            _commandListData?.OnPracticeEnd.RemoveAllListeners();
            if (_rhythmCommand != null)
            {
                _rhythmCommand.OnCommandCanceled.RemoveListener(UpdateInstructionOnMiss);
            }
        }
        private void SpeakTeaching()
        {
            if (_speaking) return;
            _speaking = true;
            int counter = 0;
            RhythmTimer.Current.OnNext.AddListener(PlaySpeech);
            void PlaySpeech()
            {
                GameSound.SpeakManager.Current.Play(_data.TeachingSound);
                RhythmTimer.Current.OnTime.AddListener(WaitUntilNextSpeak);
            }
            void WaitUntilNextSpeak()
            {
                counter++;
                if (counter == 4)
                {
                    _speaking = false;
                    RhythmTimer.Current.OnTime.RemoveListener(WaitUntilNextSpeak);
                }
            }
        }
        private void OnDestroy()
        {
            RemoveListeners();
            _data.OnSongComplete.RemoveAllListeners();
        }
    }
}
