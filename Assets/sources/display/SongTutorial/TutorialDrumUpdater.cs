using UnityEngine;

namespace PataRoad.Core.Rhythm.Command
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

        private Animator[] _animators;

        internal void Load(PracticingCommandListData commandListData)
        {
            _animators = new Animator[4];
            int i = 0;
            foreach (var drum in commandListData.FullSong)
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
            if (index == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    PlayOnIndex(i);
                }
            }
            _animators[index].Play("DrumHit");
        }
        internal void ResetHit()
        {
            for (int i = 0; i < 4; i++)
            {
                _animators[i].Play("Idle");
            }
        }
    }
}
