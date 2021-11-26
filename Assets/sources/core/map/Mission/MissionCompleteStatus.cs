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
        [SerializeField]
        int _waitTimeUntilChangeBackground;
        [SerializeField]
        Material _spriteMaterial;
        Image _screenBackground;
        private bool _bgFade;

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

            _spriteMaterial.color = Color.black;
            FindObjectOfType<BackgroundLoader>().gameObject.SetActive(false);
            Camera.main.backgroundColor = Color.white;
            StartCoroutine(ChangeBack());
        }
        System.Collections.IEnumerator ChangeBack()
        {
            yield return new WaitForSeconds(_waitTimeUntilChangeBackground);
            _screenBackground = GetComponent<Image>();
            _screenBackground.enabled = true;
            _bgFade = true;
            while (_screenBackground.color.a < 1)
            {
                var clr = _screenBackground.color;
                clr.a = Mathf.Clamp01(clr.a + 0.5f * Time.deltaTime);
                _screenBackground.color = clr;
                yield return new WaitForEndOfFrame();
            }
            _spriteMaterial.color = Color.white;
            Character.Patapons.PataponsManager.IsMovingForward = false;
        }
        private void OnDestroy()
        {
            _spriteMaterial.color = Color.white;
        }
    }
}
