using PataRoad.Core.Character;
using PataRoad.Core.Character.Patapons;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class GeneralOnStart : MonoBehaviour
    {
        [SerializeField]
        Character.Class.ClassType _classType;
        private void Start()
        {
            var pataponManager = FindObjectOfType<PataponsManager>();
            var inst = PataponGroupGenerator.GetGeneralOnlyPataponGroupInstance(_classType, pataponManager.transform, pataponManager);
            pataponManager.RegisterGroup(inst.GetComponent<PataponGroup>());
        }
    }
}
