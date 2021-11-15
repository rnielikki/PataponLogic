using UnityEngine;

namespace PataRoad.Core.Character
{
    public class CharacterSoundLoader : MonoBehaviour
    {
        public static CharacterSoundLoader Current { get; private set; }
        [SerializeReference]
        CharacterSoundsCollection _pataponSounds = new CharacterSoundsCollection();
        public CharacterSoundsCollection PataponSounds => _pataponSounds;
        [SerializeReference]
        CharacterSoundsCollection _hazoronSounds = new CharacterSoundsCollection();
        public CharacterSoundsCollection HazoronSounds => _hazoronSounds;
        void Awake()
        {
            Current = this;
        }
        private void OnDestroy()
        {
            Current = null;
        }
    }
}
