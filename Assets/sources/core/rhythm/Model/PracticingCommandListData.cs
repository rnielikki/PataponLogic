using System.Collections.Generic;
using System.Linq;

namespace PataRoad.Core.Rhythm.Command
{
    public class PracticingCommandListData : CommandListData
    {
        private readonly CommandListData _oldData;
        private readonly CommandSong _song;
        public string FullSongName { get; }
        public DrumType[] FullSong { get; }

        public const int FullPracticeCount = 4;

        private int _practiceCount;
        private readonly RhythmCommand _sender;
        public UnityEngine.Events.UnityEvent<IEnumerable<DrumType>, int> OnHit { get; } = new UnityEngine.Events.UnityEvent<IEnumerable<DrumType>, int>();
        public UnityEngine.Events.UnityEvent<IEnumerable<DrumType>, int> OnCommand { get; } = new UnityEngine.Events.UnityEvent<IEnumerable<DrumType>, int>();
        public UnityEngine.Events.UnityEvent OnPracticeEnd { get; } = new UnityEngine.Events.UnityEvent();
        internal PracticingCommandListData(RhythmCommand sender, CommandListData oldData, CommandSong song) : base(new CommandSong[] { song })
        {
            if (oldData.AvailableSongs.Contains(song))
            {
                EndPractice(false);
                return;
            }
            FullSong = _fullMap[song];
            FullSongName = string.Join(" ", FullSong).ToUpper();
            _oldData = oldData;
            _song = song;
            _sender = sender;
            _sender.OnCommandCanceled.AddListener(ResetCount);
        }
        internal override bool TryGetCommand(IEnumerable<DrumType> drums, out CommandSong song)
        {
            var gotCommand = base.TryGetCommand(drums, out song);
            if (gotCommand)
            {
                var count = drums.Count();
                OnHit.Invoke(drums, count);

                if (count == 4)
                {
                    _practiceCount++;
                    if (_practiceCount < FullPracticeCount)
                    {
                        TurnCounter.OnNextTurn.AddListener(() => OnCommand.Invoke(drums, _practiceCount));
                    }
                    else
                    {
                        TurnCounter.OnNextTurn.AddListener(() => EndPractice());
                    }
                }
            }
            else
            {
                ResetCount();
            }
            return gotCommand;
        }
        private void ResetCount()
        {
            _practiceCount = 0;
            //Display as failed
        }
        private void EndPractice(bool addSong = true)
        {
            _sender.OnCommandCanceled.RemoveListener(ResetCount);
            if (addSong) _oldData.AddSong(_song);
            _sender.SetCommandListData(_oldData);
            OnPracticeEnd.Invoke();
        }
    }
}
