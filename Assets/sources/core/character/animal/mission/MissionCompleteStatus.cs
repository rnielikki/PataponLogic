using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        private UnityEngine.InputSystem.InputAction _submitAction;
        private Story.StoryData _nextStory;

        public void LoadMissionStatus()
        {
            //Destroy(Items.ItemManager.Current.ItemDropPoint.gameObject);

            _nextStory = MissionPoint.Current.NextStory;
            if (_nextStory != null) Story.StoryLoader.Init();

            var allItemStatus = Items.ItemManager.Current.LoadItemStatus();
            var spoils = allItemStatus
                .Select(item =>
                    (item.Amount > 1) ? $"{item.Item.Name} ({item.Amount})" : $"{item.Item.Name}"
                );
            _spoilsField.text = spoils.Any() ? string.Join("\n", spoils) : "None";
            foreach (var itemStatus in allItemStatus)
            {
                Global.GlobalData.CurrentSlot.Inventory.AddMultiple(itemStatus.Item, itemStatus.Amount);
            }

            var seconds = System.TimeSpan.FromSeconds(MissionPoint.MissionCompleteTime);
            _timeField.text = seconds.ToString(@"hh\:mm\:ss");

            _spriteMaterial.color = Color.black;
            FindObjectOfType<Background.BackgroundLoader>().gameObject.SetActive(false);

            _submitAction = Global.GlobalData.Input.actions.FindAction("UI/Submit");
            if (_nextStory == null)
            {
                _submitAction.performed += LoadPatapolis;
            }
            else
            {
                _submitAction.performed += LoadStory;
            }

            Camera.main.backgroundColor = Color.white;
            StartCoroutine(ChangeBack());
        }
        System.Collections.IEnumerator ChangeBack()
        {
            yield return new WaitForSeconds(_waitTimeUntilChangeBackground);
            var screenBackground = GetComponent<Image>();
            screenBackground.enabled = true;
            while (screenBackground.color.a < 1)
            {
                var clr = screenBackground.color;
                clr.a = Mathf.Clamp01(clr.a + (0.5f * Time.deltaTime));
                screenBackground.color = clr;
                yield return new WaitForEndOfFrame();
            }
            _spriteMaterial.color = Color.white;
            Character.Patapons.PataponsManager.IsMovingForward = false;

        }
        void LoadPatapolis(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Common.GameDisplay.SceneLoadingAction.Create("Patapolis").UseTip().ChangeScene();
            _submitAction.performed -= LoadPatapolis;
        }
        void LoadStory(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Story.StoryLoader.LoadStory(_nextStory);
            _submitAction.performed -= LoadStory;
        }
        private void OnDestroy()
        {
            _spriteMaterial.color = Color.white;
            if (_submitAction != null) _submitAction.performed -= LoadStory;
        }
    }
}
