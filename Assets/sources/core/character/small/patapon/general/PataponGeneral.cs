using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons.General
{
    /// <summary>
    /// Attach this script to Patapon General prefabs
    /// </summary>
    public class PataponGeneral : MonoBehaviour
    {
        private GeneralModeActivator _generalModeActivator;
        private ParticleSystem _effect;
        public PataponGroup Group { get; private set; }

        [SerializeField]
        private IGeneralMode _generalMode;

        private void Awake()
        {
            Group = GetComponentInParent<PataponGroup>();

            //_generalMode = new HealingGeneralMode();
            _generalMode = new AttackGeneralMode();

            if (_generalMode != null) _generalModeActivator = new GeneralModeActivator(_generalMode, Group);
            _effect = transform.Find(GetComponent<Patapon>().RootName + "Effect").GetComponent<ParticleSystem>();
        }
        public void ActivateGeneralMode(CommandSong song)
        {
            if (song == _generalModeActivator?.GeneralModeSong)
            {
                _effect.Play();
                _generalModeActivator.Activate();
            }
        }
        public void CancelGeneralMode() => _generalMode?.CancelGeneralMode();
        private void OnDestroy()
        {
            CancelGeneralMode();
        }
    }
}
