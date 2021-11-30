using PataRoad.Core.Rhythm.Command;
using TMPro;
using UnityEngine;

namespace PataRoad.Common.GameDisplay
{
    class MiracleTutorial : MonoBehaviour
    {
        [SerializeField]
        private string _startMessage1;
        [SerializeField]
        private string _startMessage2;
        [SerializeField]
        private string _processingMessage;
        [SerializeField]
        private string _onMissMessage;
        [SerializeField]
        private AudioClip _miracleTeachingSound;
        [SerializeField]
        private string _endMessage;

        [SerializeField]
        private TextMeshProUGUI _instruction;
        [SerializeField]
        private TutorialDrumUpdater _command;

        private RhythmCommand _rhythmCommand;
        private PracticingMiracleListener _miracleListener;
        void Awake()
        {
            _rhythmCommand = FindObjectOfType<RhythmCommand>();
        }

        private void Start()
        {
            StartCoroutine(ShowInstruction());
        }
        System.Collections.IEnumerator ShowInstruction()
        {
            _instruction.text = _startMessage1;
            yield return new WaitForSeconds(4);
            _instruction.text = _startMessage2;
            yield return new WaitForSeconds(4);

            //"OnNextTurn" will be canceled if command is canceled.
            TurnCounter.OnTurn.AddListener(StartPracticing);
        }
        private void StartPracticing()
        {
            _instruction.text = _processingMessage;

            _miracleListener = _rhythmCommand.ToMiraclePracticeMode();
            _command.LoadForMiracle();
            GameSound.SpeakManager.Current.Play(_miracleTeachingSound);
            _miracleListener.OnMiracleDrumHit.AddListener(UpdateDrumStatus);
            _miracleListener.OnMiracleDrumMiss.AddListener(ShowMissInstruction);
            _miracleListener.OnMiraclePerformed.AddListener(UpdateInstruction);
            _miracleListener.OnMiraclePracticingEnd.AddListener(EndInstruction);

            TurnCounter.OnTurn.RemoveListener(StartPracticing);
        }
        private void UpdateDrumStatus(int count)
        {
            _command.PlayOnIndex(count - 1);
        }
        private void UpdateInstruction(int practicingCount)
        {
            _instruction.text = $"{_processingMessage} ({PracticingMiracleListener.FullPracticingCount - practicingCount} time(s) left)";
            _command.ResetHit();
        }

        private void ShowMissInstruction()
        {
            _instruction.text = _onMissMessage;
            _command.ResetHit();
        }
        public void EndInstruction()
        {
            _instruction.text = _endMessage;
            _command.gameObject.SetActive(false);
            StartCoroutine(EndInstructionOnTime());
            System.Collections.IEnumerator EndInstructionOnTime()
            {
                yield return new WaitForSeconds(8);
                Destroy(gameObject);
            }
        }
    }
}
