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

        private const string _percentFormat = "p0";
        private CharacterNavigator _nav;

        public void Open(Object sender, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _nav = sender as CharacterNavigator;
            _nav.Freeze();
            gameObject.SetActive(true);
            UpdateText(_nav.Current.GetComponent<PataponData>().AttackTypeResistance);
        }
        public void Close()
        {
            _nav.Defrost();
            gameObject.SetActive(false);
        }

        private void UpdateText(AttackTypeResistance resistance)
        {
            _crushText.text = resistance.CrushMultipler.ToString(_percentFormat);
            _slashText.text = resistance.SlashMultipler.ToString(_percentFormat);
            _stabText.text = resistance.StabMultipler.ToString(_percentFormat);
            _soundText.text = resistance.SoundMultipler.ToString(_percentFormat);
            _magicText.text = resistance.MagicMultipler.ToString(_percentFormat);
        }
    }
}
