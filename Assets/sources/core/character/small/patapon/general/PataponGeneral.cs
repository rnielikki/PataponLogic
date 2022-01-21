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
        private string _generalName;
        public string GeneralName => _generalName;

        [SerializeField]
        private AudioClip _generalModeSound;
        private Patapon _selfPatapon;

        private void Start()
        {
            Group = GetComponentInParent<PataponGroup>();
            _selfPatapon = GetComponent<Patapon>();
            _generalEffect = GetGeneralEffect(_selfPatapon.Type);

            EquipGeneralMode(Global.GlobalData.CurrentSlot.PataponInfo.GetGeneralMode(_selfPatapon.Type));

            _generalEffect.StartSelfEffect(_selfPatapon);
            _generalEffect.StartGroupEffect(Group.Patapons);
        }

        public void ActivateGeneralMode(CommandSong song)
        {
            if (_selfPatapon == null || _selfPatapon.StatusEffectManager.IsOnStatusEffect) return;
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
            if (Group != null) _generalEffect?.EndGroupEffect(Group.Patapons);
        }
        //------------ Loads general effect
        public static IGeneralEffect GetGeneralEffect(Class.ClassType type) => type switch
        {
            Class.ClassType.Tatepon => new RahGashaponEffect(),
            Class.ClassType.Dekapon => new TonKamponEffect(),
            Class.ClassType.Robopon => new KonKimponEffect(),
            Class.ClassType.Kibapon => new HataponEffect(),
            Class.ClassType.Toripon => new HataponEffect(),
            Class.ClassType.Yaripon => new PrincessEffect(),
            Class.ClassType.Megapon => new PanPakaponEffect(),
            Class.ClassType.Yumipon => new SukoponEffect(),
            Class.ClassType.Mahopon => new MedenEffect(),
            _ => throw new System.ArgumentException("Cannot get \"Any\" type of general data.")
        };
    }
}
