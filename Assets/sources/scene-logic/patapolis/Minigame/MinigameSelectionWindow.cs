using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Patapolis.Minigame
{
    internal class MinigameSelectionWindow : SelectionWindow
    {
        [SerializeField]
        MinigameMaterialWindow _materialWindow;
        private MinigameSelectionButton[] _buttons;
        [SerializeField]
        [Tooltip("Although it says it's animation curve, it's not related to animation at all.")]
        AnimationCurve _requirementCurve;
        internal AnimationCurve RequirementCurve => _requirementCurve;

        private void Start()
        {
            _buttons = GetComponentsInChildren<MinigameSelectionButton>();
        }
        protected override void InitButtons()
        {
            foreach (var obj in _buttons)
            {
                obj.GetComponent<Button>().onClick.AddListener(() => _materialWindow.Open(this, obj));
            }
        }
        protected override void ResetButtons()
        {
            foreach (var obj in _buttons)
            {
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
    }
}
