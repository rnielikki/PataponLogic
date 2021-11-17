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
        private PataponsManager _pataponsManager;

        public static bool ShoutedOnThisTurn { get; set; }
        private IGeneralEffect _generalEffect;

        [SerializeField]
        private AudioClip _generalModeSound;
        private Patapon _selfPatapon;

        //-------------------------------------------[testing]
        private static int _counter;
        private static IGeneralMode[] _modes = new IGeneralMode[]
        {
            new AttackGeneralMode(),
            new HealingGeneralMode(),
            new DefenceGeneralMode()
        };
        //-----------------------------------

        private void Awake()
        {
            Group = GetComponentInParent<PataponGroup>();
            _selfPatapon = GetComponent<Patapon>();
            _generalEffect = _selfPatapon.GetGeneralEffect();

            //-------------------------------------------[testing]
            IGeneralMode gen;
            if (_counter < _modes.Length) gen = _modes[_counter];
            else gen = null;
            if (gen != null) _generalModeActivator = new GeneralModeActivator(gen, Group);
            _counter++;
            //-----------------------------------

            _effect = transform.Find(GetComponent<Patapon>().RootName + "Effect").GetComponent<ParticleSystem>();
        }

        public void ActivateGeneralMode(CommandSong song)
        {
            if (_selfPatapon.StatusEffectManager.OnStatusEffect) return;
            if (_pataponsManager == null) _pataponsManager = GetComponentInParent<PataponsManager>();
            if (song == _generalModeActivator?.GeneralModeSong)
            {
                _effect.Play();
                if (!_generalModeActivator.OnGeneralModeCombo && !ShoutedOnThisTurn)
                {
                    TurnCounter.OnNextTurn.AddListener(() => GameSound.SpeakManager.Current.Play(_generalModeSound));
                    ShoutedOnThisTurn = true;
                }
                _generalModeActivator.Activate(_pataponsManager.Groups);
            }
        }
        public void CancelGeneralMode() => _generalModeActivator?.Cancel();

        private void OnDestroy()
        {
            CancelGeneralMode();
        }
    }
}
