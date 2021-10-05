using UnityEngine;

namespace Core.Rhythm.Display
{
    /// <summary>
    /// This simple code is for managing multiple input display at once, e.g. Miracle event. It prevents adding 4 objects separately for event on Unity Editor.
    /// </summary>
    public class RhythmDrumDisplayManager : MonoBehaviour
    {
        RhythmInputDisplay[] _displays;
        public void ShowAll() => SetAllEnableState(true);
        public void HideAll() => SetAllEnableState(false);
        private void Awake()
        {
            _displays = GetComponentsInChildren<RhythmInputDisplay>();
        }

        private void SetAllEnableState(bool state)
        {
            foreach (var display in _displays) display.enabled = state;
        }
    }
}
