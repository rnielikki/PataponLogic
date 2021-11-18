using UnityEngine;

namespace PataRoad.Core.Items
{
    /// <summary>
    /// Represents any equipment DATA that Patapon (or enemy) use, like spear, shield, cape, helm. Attached to every equipment prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "general-mode-by-number", menuName = "KeyItemData/GeneralModeData")]
    public class GeneralModeData : KeyItemData
    {
        public override string Group => "GeneralMode";
        [SerializeField]
        private GameObject _generalModeObject;
        public GameObject EffectObject => _generalModeObject;
    }
}
