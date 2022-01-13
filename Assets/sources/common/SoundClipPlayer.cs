using UnityEngine;

namespace PataRoad.Common
{
    class SoundClipPlayer : MonoBehaviour
    {
        [SerializeField]
        AudioClip[] _audioClips;
        public void PlaySoundInIndex(int index)
        {
            if (index >= 0 && index < _audioClips.Length)
            {
                Core.Global.GlobalData.Sound.PlayInScene(_audioClips[index]);
            }
        }
    }
}
