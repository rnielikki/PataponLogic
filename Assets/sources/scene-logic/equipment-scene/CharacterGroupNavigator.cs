using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class CharacterGroupNavigator : MonoBehaviour
    {
        [SerializeField]
        private Sprite _groupBackground;
        [SerializeField]
        private GameObject _characterBackground;
        [SerializeField]
        private AudioClip _selectSound;
        private AudioSource _audioSource;

        private CharacterNavigator _currentNavigator;
        private List<CharacterNavigator> _characterNavs = new List<CharacterNavigator>();
        private int _index;
        // Start is called before the first frame update
        public void Init()
        {
            _audioSource = GetComponent<AudioSource>();
            foreach (Transform child in transform)
            {
                var comp = child.gameObject.AddComponent<CharacterNavigator>();
                comp.Init(this, _groupBackground, _characterBackground);
                _characterNavs.Add(comp);
                Instantiate(_groupBackground);
            }
            _currentNavigator = _characterNavs[0];
            _currentNavigator.SelectThis();
        }
        public void MoveTo(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                    _index = (_index + 1) % _characterNavs.Count;
                    break;
                case MoveDirection.Right:
                    _index = (_index - 1 + _characterNavs.Count) % _characterNavs.Count;
                    break;
                default:
                    return;
            }
            _currentNavigator = _characterNavs[_index];
            _currentNavigator.SelectThis();
            _audioSource.PlayOneShot(_selectSound);
        }
    }
}
