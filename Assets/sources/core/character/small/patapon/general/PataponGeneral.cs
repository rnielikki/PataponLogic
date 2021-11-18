using PataRoad.Core.Rhythm.Command;
using UnityEngine;
using UnityEngine.Scripting;

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

        [SerializeField]
        Items.GeneralModeData _generalModeData;

        private void Awake()
        {
            Group = GetComponentInParent<PataponGroup>();
            _selfPatapon = GetComponent<Patapon>();
            _generalEffect = _selfPatapon.GetGeneralEffect();

            EquipGeneralMode(_generalModeData);

        }
        private void Start()
        {
            _generalEffect.StartSelfEffect(_selfPatapon);
            _generalEffect.StartGroupEffect(Group.Patapons);
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
        public void EquipGeneralMode(Items.GeneralModeData data)
        {
            if (data == null) return;
            var effectObject = Instantiate(data.EffectObject, transform.Find(GetComponent<Patapon>().RootName));
            _effect = effectObject.GetComponent<ParticleSystem>();
            var mode = effectObject.GetComponent<GeneralMode>();
            if (mode != null) _generalModeActivator = new GeneralModeActivator(mode, Group);
        }
        public void CancelGeneralMode() => _generalModeActivator?.Cancel();

        private void OnDestroy()
        {
            CancelGeneralMode();
            _generalEffect.EndGroupEffect(Group.Patapons);
        }
    }
}
