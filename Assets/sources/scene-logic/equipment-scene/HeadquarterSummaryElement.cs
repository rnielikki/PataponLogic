using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class HeadquarterSummaryElement : SummaryElement
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent<HeadquarterSummaryElement> _onSubmit;
        public UnityEngine.Events.UnityEvent<HeadquarterSummaryElement> OnSubmit => _onSubmit;

        [SerializeField]
        string _bindingName;

        [SerializeField]
        string _additionalData;
        public string AdditionalData => _additionalData;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(_bindingName) && Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName(_bindingName, out string result))
            {
                GetComponentInChildren<Text>().text += $" ({result})";
            }
            if (!string.IsNullOrEmpty(_additionalData))
            {
                switch (_additionalData)
                {
                    case "Boss":
                        UpdateImageAndText(Core.Global.GlobalData.PataponInfo.BossToSummon, "Boss to summon");
                        break;
                    case "Music":
                        UpdateImageAndText(Core.Global.GlobalData.PataponInfo.CustomMusic, "Music theme");
                        break;
                }
            }
        }
        private void UpdateImageAndText(Core.Items.IItem item, string defaultText)
        {
            GetComponentInChildren<Text>().text = item?.Name ?? defaultText;
            transform.Find("Image").GetComponent<Image>().sprite = item?.Image;
        }
        private void OnDestroy()
        {
            _onSubmit?.RemoveAllListeners();
        }
    }
}
