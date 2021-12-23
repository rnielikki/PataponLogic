using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visualize the <see cref="RhythmTimer"/> on the screen, with opacity.
/// </summary>
namespace PataRoad.Core.Rhythm.Display
{
    public class RhythmDisplay : MonoBehaviour
    {
        Image _image;
        Image _hitImage;
        void Awake()
        {
            _image = GetComponent<Image>();
            _hitImage = transform.Find("RhythmHitDisplay").GetComponent<Image>();
            _hitImage.color = Color.clear;
        }

        public void ShowHit(RhythmInputModel model)
        {
            if (model.Status != DrumHitStatus.Miss) _hitImage.color = Color.white;
            RhythmTimer.Current.OnNextHalfTime.AddListener(() => _hitImage.color = Color.clear);
        }

        // Update is called once per frame
        void Update()
        {
            _image.color = new Color(1, 1, 1, (float)RhythmTimer.GetTiming() / RhythmTimer.Frequency);
        }

        public void Stop() => gameObject.SetActive(false);
    }
}
