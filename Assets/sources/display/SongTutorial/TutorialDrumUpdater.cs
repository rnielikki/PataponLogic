using PataRoad.Core.Rhythm;
using PataRoad.Core.Rhythm.Command;
using System.Linq;
using UnityEngine;

namespace PataRoad.GameDisplay
{
    public class TutorialDrumUpdater : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pata;
        [SerializeField]
        private GameObject _pon;
        [SerializeField]
        private GameObject _chaka;
        [SerializeField]
        private GameObject _don;
        private int _maxLength;
        private int _maxIndex => _maxLength - 1;

        private Animator[] _animators;

        internal void Load(PracticingCommandListData commandListData)
        {
            LoadDrums(commandListData.FullSong);
        }
        internal void LoadForMiracle()
        {
            LoadDrums(new[] { DrumType.Don, DrumType.Don, DrumType.Don, DrumType.Don, DrumType.Don });
        }
        private void LoadDrums(System.Collections.Generic.IEnumerable<DrumType> drums)
        {
            _maxLength = drums.Count();
            _animators = new Animator[_maxLength];
            int i = 0;
            foreach (var drum in drums)
            {
                GameObject drumToInstantiate;
                switch (drum)
                {
                    case DrumType.Pata:
                        drumToInstantiate = _pata;
                        break;
                    case DrumType.Pon:
                        drumToInstantiate = _pon;
                        break;
                    case DrumType.Chaka:
                        drumToInstantiate = _chaka;
                        break;
                    case DrumType.Don:
                        drumToInstantiate = _don;
                        break;
                    default:
                        throw new System.InvalidOperationException("That's all drum we have. Should not reach here.");
                }
                _animators[i] = Instantiate(drumToInstantiate, transform).GetComponent<Animator>();
                i++;
            }
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
        internal void PlayOnIndex(int index)
        {
            if (index == _maxIndex)
            {
                for (int i = 0; i < _maxIndex; i++)
                {
                    PlayOnIndex(i);
                }
            }
            _animators[index].Play("DrumHit");
        }
        internal void ResetHit()
        {
            for (int i = 0; i < _maxLength; i++)
            {
                _animators[i].Play("Idle");
            }
        }
    }
}
