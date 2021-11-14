using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace PataRoad.Core.Map
{
    public class MissionCompleteStatus : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        Text _timeField;
        [SerializeField]
        Text _spoilsField;

        public void LoadMissionStatus()
        {
            Destroy(Items.ItemManager.Current.ItemDropPoint.gameObject);
            var spoils = Items.ItemManager.Current.LoadItemStatus()
                .Select(item =>
                    (item.Amount > 1) ? $"{item.Item.Name} ({item.Amount})" : $"{item.Item.Name}"
                );
            _spoilsField.text = spoils.Any() ? string.Join("\n", spoils) : "None";
            var seconds = System.TimeSpan.FromSeconds(MissionPoint.MissionCompleteTime);
            _timeField.text = seconds.ToString(@"hh\:mm\:ss");
        }
    }
}
