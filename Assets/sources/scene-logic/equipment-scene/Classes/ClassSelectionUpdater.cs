using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    class ClassSelectionUpdater : MonoBehaviour
    {
        [SerializeField]
        Text _className;
        [SerializeField]
        Text _description;
        [SerializeField]
        Text _generalName;
        [SerializeField]
        Text _selfEffect;
        [SerializeField]
        Text _groupEffect;
        internal void UpdateDescription(ClassSelectionInfo classSelectionInfo)
        {
            _className.text = classSelectionInfo.ClassType.ToString();
            _description.text = classSelectionInfo.ClassDescription;
            _generalName.text = classSelectionInfo.GeneralName;
            _selfEffect.text = classSelectionInfo.SelfEffectDescription;
            _groupEffect.text = classSelectionInfo.GroupEffectDescription;
        }
    }
}
