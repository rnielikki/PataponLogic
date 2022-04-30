using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Map.Environment
{
    abstract class TriggerZone : MonoBehaviour
    {
        protected bool _enabled { get; private set; }
        private readonly List<Character.ICharacter> _characters = new List<Character.ICharacter>();
        private readonly HashSet<Character.ICharacter> _enteredCharacters = new HashSet<Character.ICharacter>();
        private readonly List<Character.ICharacter> _removeTarget = new List<Character.ICharacter>();

        protected abstract void OnFirstEnter();
        protected abstract void OnLastExit();
        protected abstract void OnStarted(IEnumerable<Character.ICharacter> currentStayingCharacters);
        protected abstract void OnStay(IEnumerable<Character.ICharacter> currentStayingCharacters);
        protected abstract void OnEnded(IEnumerable<Character.ICharacter> currentStayingCharacters);

        private Vector2 _center;
        private Vector2 _size;
        private LayerMask _layerMask;

        [SerializeField]
        protected string[] _layerMaskNames = { "patapons", "hazorons", "bosses" };

        protected void Init()
        {
            var collider = GetComponent<Collider2D>();
            _center = collider.bounds.center;
            _size = collider.bounds.size + (Vector3.one * 0.5f);
            _layerMask = LayerMask.GetMask(_layerMaskNames);
        }
        protected void SetEnable()
        {
            if (!_enabled)
            {
                Rhythm.RhythmTimer.Current.OnTime.AddListener(OnUpdating);
                _enabled = true;
                OnStarted(_characters);
                var targets = Physics2D.OverlapBoxAll(_center, _size, transform.eulerAngles.z, _layerMask);
                _characters.Clear();
                foreach (var target in targets)
                {
                    if (!target.CompareTag("SmallCharacter")) continue;
                    var character = target.GetComponentInParent<Character.ICharacter>();
                    if (character != null) _characters.Add(character);
                }
                if (_characters.Count > 0) OnFirstEnter();
            }
        }
        protected void SetDisable()
        {
            if (_enabled)
            {
                OnEnded(_characters);
                _enabled = false;
                if (_characters.Count > 0) OnLastExit();
                _characters.Clear();
            }
            Rhythm.RhythmTimer.Current.OnTime.RemoveListener(OnUpdating);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_enabled || _layerMask != (_layerMask | (1 << collision.gameObject.layer))) return;
            if (collision.gameObject.CompareTag("SmallCharacter"))
            {
                var receiver = collision.gameObject.GetComponentInParent<Character.ICharacter>();
                if (receiver != null && !_characters.Contains(receiver))
                {
                    if (_characters.Count == 0)
                    {
                        OnFirstEnter();
                    }
                    _characters.Add(receiver);
                    if (receiver.OnAfterDeath != null && !_enteredCharacters.Contains(receiver))
                    {
                        _enteredCharacters.Add(receiver);
                        receiver.OnAfterDeath.AddListener(() =>
                        {
                            if (_characters.Contains(receiver))
                            {
                                _removeTarget.Add(receiver);
                            }
                        });
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_enabled) return;
            if (collision.gameObject.CompareTag("SmallCharacter"))
            {
                var receiver = collision.gameObject.GetComponentInParent<Character.ICharacter>();
                if (receiver != null && _characters.Contains(receiver))
                {
                    _removeTarget.Add(receiver);
                }
            }
        }
        private void OnUpdating()
        {
            if (!_enabled)
            {
                SetDisable();
                return;
            }
            OnStay(_characters);
            //collection shouldn't be changed while iterating
            if (_removeTarget.Count > 0)
            {
                foreach (var character in _removeTarget)
                {
                    _characters.Remove(character);
                }
                if (_characters.Count == 0)
                {
                    OnLastExit();
                }
                _removeTarget.Clear();
            }
        }
    }
}
