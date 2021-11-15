using UnityEngine;

namespace PataRoad.Core.Character
{
    [System.Serializable]
    public class CharacterSoundsCollection
    {
        [SerializeField]
        AudioClip _onFire;
        public AudioClip OnFire => _onFire;

        [SerializeField]
        AudioClip _onSleep;
        public AudioClip OnSleep => _onSleep;

        [SerializeField]
        AudioClip _onDead;
        public AudioClip OnDead => _onDead;
    }
}
