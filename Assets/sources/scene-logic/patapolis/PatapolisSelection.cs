using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.SceneLogic.Patapolis
{
    internal class PatapolisSelection : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer[] _onSelectedImages;
        [SerializeField]
        AudioClip _selectSound;

        [SerializeReference]
        SpriteRenderer[] _imagesForReplace;
        [SerializeReference]
        Sprite[] _imagesWillReplaceOnSelect;
        Sprite[] _imagesWillReplaceOnDeselect;

        [SerializeField]
        GameObject _label;

        [SerializeField]
        UnityEvent _onSelect;
        [SerializeField]
        UnityEvent _onPerformed;
        internal void Init()
        {
            if (_imagesWillReplaceOnSelect.Length != _imagesForReplace.Length)
            {
                throw new MissingComponentException("ImageForReplace length must be same as ImageWillReplaceOnSelect");
            }
            _imagesWillReplaceOnDeselect = _imagesForReplace.Select(spr => spr.sprite).ToArray();
            _label.SetActive(false);
        }
        internal void Select()
        {
            foreach (var img in _onSelectedImages) img.enabled = true;
            for (int i = 0; i < _imagesForReplace.Length; i++)
            {
                _imagesForReplace[i].sprite = _imagesWillReplaceOnSelect[i];
            }
            Core.Global.GlobalData.Sound.PlayInScene(_selectSound);
            _label.SetActive(true);
            _onSelect?.Invoke();
        }
        internal void Deselect()
        {
            foreach (var img in _onSelectedImages) img.enabled = false;
            for (int i = 0; i < _imagesForReplace.Length; i++)
            {
                _imagesForReplace[i].sprite = _imagesWillReplaceOnDeselect[i];
            }
            _label.SetActive(false);
        }
        internal void Perform() => _onPerformed?.Invoke();
    }
}

