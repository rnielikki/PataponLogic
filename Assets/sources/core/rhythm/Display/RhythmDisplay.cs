using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visualize the <see cref="RhythmTimer"/> on the screen, with opacity.
/// </summary>
namespace Core.Rhythm.Display
{
    public class RhythmDisplay : MonoBehaviour
    {
        Image _image;
        void Awake()
        {
            _image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            _image.color = new Color(1, 1, 1, (float)RhythmTimer.GetTiming() / RhythmTimer.Frequency);
        }
    }
}
