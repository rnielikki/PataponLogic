using UnityEngine;
using UnityEngine.Events;

namespace PataRoad.Core.Rhythm.Display
{
    /// <summary>
    /// This simple code is for managing multiple input display at once, e.g. Miracle event. It prevents adding 4 objects separately for event on Unity Editor.
    /// </summary>
    public class RhythmDrumDisplayManager : MonoBehaviour
    {
        public UnityEvent<RhythmInputModel> OnAnyDrumInput;
        RhythmInputDisplay[] _displays;
        /// <summary>
        /// Shows current *display* status
        /// </summary>
        public void ShowAll() => SetAllDisplayState(true);
        /// <summary>
        /// Hides current *display* status. DOESN'T STOP listening and sound playing.
        /// </summary>
        public void HideAll() => SetAllDisplayState(false);
        private void Awake()
        {
            _displays = GetComponentsInChildren<RhythmInputDisplay>();
            foreach (var input in GetComponentsInChildren<RhythmInput>())
            {
                input.OnDrumHit.AddListener((model) => OnAnyDrumInput.Invoke(model));
            }
        }

        private void SetAllDisplayState(bool state)
        {
            foreach (var display in _displays) display.enabled = state;
        }
        public void Deactive() => gameObject.SetActive(false);
        private void OnDestroy()
        {
            OnAnyDrumInput.RemoveAllListeners();
        }
    }
}
