using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class CharacterDetail : MonoBehaviour
    {
        [SerializeField]
        Text _crushText;
        [SerializeField]
        Text _slashText;
        [SerializeField]
        Text _stabText;
        [SerializeField]
        Text _soundText;
        [SerializeField]
        Text _magicText;

        [Header("elemental attks")]
        [SerializeField]
        Text _fireText;
        [SerializeField]
        Text _iceText;
        [SerializeField]
        Text _thunderText;

        [Header("attack types")]
        [SerializeField]
        Text _attackType;
        [SerializeField]
        Text _elementalAttackType;

        private const string _percentFormat = "p0";
        private CharacterNavigator _nav;

        public void Open(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as CharacterNavigator;
            _nav.Freeze();
            gameObject.SetActive(true);
            UpdateText(_nav.Current.GetComponent<PataponData>());
        }
        public void Close()
        {
            _nav.Defrost();
            gameObject.SetActive(false);
        }

        private void UpdateText(PataponData pataponData)
        {
            AttackTypeResistance resistance = pataponData.AttackTypeResistance;
            _crushText.text = resistance.CrushMultipler.ToString(_percentFormat);
            _slashText.text = resistance.SlashMultipler.ToString(_percentFormat);
            _stabText.text = resistance.StabMultipler.ToString(_percentFormat);
            _soundText.text = resistance.SoundMultipler.ToString(_percentFormat);
            _magicText.text = resistance.MagicMultipler.ToString(_percentFormat);

            _fireText.text = resistance.FireMultiplier.ToString(_percentFormat);
            _iceText.text = resistance.IceMultiplier.ToString(_percentFormat);
            _thunderText.text = resistance.ThunderMultiplier.ToString(_percentFormat);

            _attackType.text = pataponData.EquipmentManager.Weapon.AttackType.ToString();
            if (pataponData.Type == Core.Character.Class.ClassType.Mahopon)
            {
                _elementalAttackType.text = ((Core.Character.Equipments.Weapons.ElementalAttackType)
                    Core.Global.GlobalData.CurrentSlot.PataponInfo.GetAttackTypeIndex(Core.Character.Class.ClassType.Mahopon)).ToString();
            }
            else
            {
                _elementalAttackType.text = pataponData.ElementalAttackType.ToString();
            }
        }
    }
}
