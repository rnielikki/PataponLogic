using UnityEngine;

namespace PataRoad.Story
{
    class SoundOnEnable : MonoBehaviour
    {
        [SerializeField]
        AudioClip _sound;
        private void Start()
        {
            Core.Global.GlobalData.Sound.PlayInScene(_sound);
        }
    }
}
