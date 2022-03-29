using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis.ItemExchange
{
    public class ExchangeMaterialLoader : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _title;
        ExchangeMaterialSelection[] _selections;
        internal void LoadMaterials(ItemExchangeToggleMenu menu)
        {
            _title.text = menu.IsGem ? "Gem" : menu.Material.ToString();
            if (_selections == null)
            {
                _selections = GetComponentsInChildren<ExchangeMaterialSelection>(true);
            }
            for (int i = 0; i < 5; i++)
            {
                _selections[i].Init(menu, i);
            }
        }
    }
}