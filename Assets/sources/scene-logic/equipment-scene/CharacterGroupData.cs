using PataRoad.Core.Character.Class;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class CharacterGroupData : MonoBehaviour
    {
        public ClassType Type { get; private set; }
        public ClassMetaData ClassData { get; set; }
        public void Init(ClassType type)
        {
            Type = type;
            ClassData = ClassMetaData.Get(type);
        }
    }
}
