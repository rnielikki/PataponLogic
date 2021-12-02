using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class EquipmentDisplay : MonoBehaviour
    {
        Text _text;
        private Common.Navigator.SpriteActionMap _map;
        private CharacterNavigator _nav;
        [SerializeField]
        private GameObject _child;
        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponentInChildren<Text>(true);
            _map = GetComponent<Common.Navigator.SpriteActionMap>();
            _map.enabled = false;
        }

        // Update is called once per frame
        public void ShowEquipment(Object sender)
        {
            _nav = sender as CharacterNavigator;
            var data = _nav?.Current?.GetComponent<Core.Character.PataponData>();
            if (data == null) return;

            _map.enabled = true;
            enabled = true;
            _nav.enabled = false;
            _child.SetActive(true);

            _text.text = $"{data.Type} / {data.WeaponName} / {data.ProtectorName}";
        }
        public void HideEquipment(Object sender)
        {
            _nav.enabled = true;
            _nav.Current.SelectThis();
            _map.enabled = false;
            _child.SetActive(false);
        }
    }
}
