using UnityEngine;

namespace PataRoad.Common
{
    class AudioSourceFixedUpdate : MonoBehaviour
    {
        [SerializeField]
        AudioSource _audioSource;
        private void Awake()
        {
            _audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
        }
    }
}
