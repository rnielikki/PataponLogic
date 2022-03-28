using UnityEngine;

namespace PataRoad.Story.Actions
{
    /// <summary>
    /// Changes music when story is going.
    /// </summary>
    class StoryMusicChanger : MonoBehaviour
    {
        AudioSource _audioSource;
        public void StopMusic()
        {
            if (_audioSource == null)
            {
                _audioSource = FindObjectOfType<StorySceneInfo>().AudioSource;
            }
            _audioSource.Stop();
        }
        public void ChangeMusic(AudioClip clip) => SetMusic(clip, true);
        public void ChangeMusicAndPlayOnce(AudioClip clip) => SetMusic(clip, false);
        public void SetMusic(AudioClip clip, bool repeat)
        {
            StopMusic();
            _audioSource.loop = repeat;
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
