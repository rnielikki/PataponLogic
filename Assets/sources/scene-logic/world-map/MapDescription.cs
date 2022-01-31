using PataRoad.Core.Map;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.WorldMap
{
    class MapDescription : MonoBehaviour
    {
        [SerializeField]
        Text _title;
        [SerializeField]
        Text _description;
        private MapDataContainer _target;
        public void ShowDescription()
        {
            gameObject.SetActive(true);
            if (_target != null) UpdateDescription();
        }
        public void HideDescription()
        {
            gameObject.SetActive(false);
        }
        public void UpdateDescription(MapDataContainer target)
        {
            _target = target;
            if (gameObject.activeSelf) UpdateDescription();
        }
        private void UpdateDescription()
        {
            _title.text = _target.GetNameWithLevel();
            _description.text = _target.Description;
        }
    }
}
