using UnityEngine;

namespace PataRoad.Core.Character
{
    public class StructurePhaseCollection : ScriptableObject
    {
        [SerializeField]
        Sprite[] _images;
        public int Length => _images.Length;
        [SerializeField]
        [Tooltip("this overwrites default color over health")]
        private Gradient _colorOverHealth;
        public Gradient ColorOverHealth => _colorOverHealth;

        public Sprite Resolve(int index)
        {
            if (index < 0)
            {
                return _images[0];
            }
            else if (index >= Length)
            {
                return _images[Length - 1];
            }
            else return _images[index];
        }
    }
}