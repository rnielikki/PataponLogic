using UnityEngine;
using PataRoad.Core.Rhythm.Command;
using TMPro;
using PataRoad.Core.Rhythm;
using System.Text;

namespace PataRoad.GameDisplay
{
    public class SongTutorial : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private Core.Items.SongItemDropData _data;
        // Start is called before the first frame update
        private RhythmCommand _rhythmCommand;
        private PracticingCommandListData _commandListData;
        void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
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
            _text.text = _data.StartMessage;
            yield return new WaitForSeconds(4);
            _rhythmCommand.enabled = true;
            _commandListData = _rhythmCommand.ToPracticeMode(song);
            _commandListData.OnHit.AddListener(UpdateInstruction);
            _commandListData.OnCommand.AddListener(UpdateInstructionOnCommand);
            _commandListData.OnPracticeEnd.AddListener(EndInstruction);
            _rhythmCommand.OnCommandCanceled.AddListener(UpdateInstructionOnMiss);
            _text.text = _data.ProcessingMessage;
        }
        private void UpdateInstruction(System.Collections.Generic.IEnumerable<DrumType> drums, int count)
        {
            var str = new StringBuilder(_data.ProcessingMessage);
            str.Append("\n");
            foreach (var d in drums)
            {
                str.Append(d.ToString());
            }
            str.Append(" (" + count + ")");
            _text.text = str.ToString();
        }
        private void UpdateInstructionOnCommand(System.Collections.Generic.IEnumerable<DrumType> drums, int count)
        {
            _text.text = $"Cool! {PracticingCommandListData.FullPracticeCount - count} more time!  {_commandListData.FullSong}";
        }

        private void UpdateInstructionOnMiss()
        {
            _text.text = "Keep calm and press: " + _commandListData.FullSong;
        }
        public void EndInstruction()
        {
            StopAllCoroutines();
            RemoveListeners();
            _text.text = _data.EndMessage;
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
            _commandListData.OnHit.RemoveAllListeners();
            _commandListData.OnCommand.RemoveAllListeners();
            _commandListData.OnPracticeEnd.RemoveAllListeners();
            _rhythmCommand.OnCommandCanceled.RemoveListener(UpdateInstructionOnMiss);
        }
        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}
