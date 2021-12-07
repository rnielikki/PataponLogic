using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// This will activate to open available classes.
    /// </summary>
    [CreateAssetMenu(fileName = "ClassMemory", menuName = "Item/KeyItemData/ClassMemory")]
    class ClassMemoryData : KeyItemData
    {
        public override string Group => "Class";
        [SerializeField]
        private Character.Class.ClassType _class;
        public Character.Class.ClassType Class => _class;
    }
}
