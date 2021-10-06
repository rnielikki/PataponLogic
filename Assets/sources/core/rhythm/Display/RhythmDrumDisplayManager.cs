using UnityEngine;
using UnityEngine.Events;

namespace Core.Rhythm.Display
{
    /// <summary>
    /// This simple code is for managing multiple input display at once, e.g. Miracle event. It prevents adding 4 objects separately for event on Unity Editor.
    /// </summary>
    public class RhythmDrumDisplayManager : MonoBehaviour
    {
        public UnityEvent<RhythmInputModel> OnAnyDrumInput;
        RhythmInputDisplay[] _displays;
        public void ShowAll() => SetAllEnableState(true);
        public void HideAll() => SetAllEnableState(false);
        private void Awake()
        {
            _displays = GetComponentsInChildren<RhythmInputDisplay>();
            var inputs = GetComponentsInChildren<RhythmInput>();
            foreach (var input in inputs)
            {
                input.OnDrumHit.AddListener((model) => OnAnyDrumInput.Invoke(model));
            }
        }

        private void SetAllEnableState(bool state)
        {
            foreach (var display in _displays) display.enabled = state;
        }
        private void OnDestroy()
        {
            OnAnyDrumInput.RemoveAllListeners();
        }
    }
}
