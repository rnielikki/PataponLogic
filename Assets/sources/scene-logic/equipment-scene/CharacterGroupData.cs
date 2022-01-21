using PataRoad.Core.Character.Class;
using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class CharacterGroupData : MonoBehaviour
    {
        public ClassType Type { get; private set; }
        public ClassAttackEquipmentData ClassData { get; set; }
        public void Init(ClassType type)
        {
            Type = type;
            ClassData = ClassAttackEquipmentData.Get(type);
        }
    }
}
