using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    /// <summary>
    /// Represents Wind visualisation.
    /// </summary>
    public class WindImage : MonoBehaviour
    {
        [SerializeField]
        private TrailRenderer _trail;
        [SerializeField]
        private Animator _animator;
        private Color _color = new Color(1, 1, 1);
        [SerializeField]
        private AudioSource _windSound;
        public void Init()
        {
            _color = new Color(1, 1, 1);
            PlayAnimation();
        }
        public void ClearTrail()
        {
            _trail.Clear();
        }
        /// <summary>
        /// Update wind direction and alpha.
        /// </summary>
        /// <param name="windPower">The current wind value.</param>
        public void Visualise(float windPower, float percentage)
        {
            ChangeDirection(windPower);
            ChangeAlpha(Mathf.Abs(percentage));
            ChangeSound(percentage);
        }

        private void ChangeDirection(float windPower)
        {
            var scale = transform.parent.localScale;

            if (windPower > 0 && scale.x != 1)
            {
                ResetTrail();
                _animator.enabled = true;
                PlayAnimation();
                scale.x = 1;
                transform.parent.localScale = scale;
            }
            else if (windPower < 0 && scale.x != -1)
            {
                ResetTrail();
                _animator.enabled = true;
                PlayAnimation();
                scale.x = -1;
                transform.parent.localScale = scale;
            }
            else if (windPower == 0 && scale.x != 0)
            {
                _animator.enabled = false;
                ResetTrail();
                scale.x = 0;
                transform.parent.localScale = scale;
            }
        }

        private void ChangeAlpha(float alpha)
        {
            _color.a = alpha;
            _trail.startColor = _color;
            _trail.endColor = _color;
        }
        private void ChangeSound(float percentage)
        {
            if (percentage == 0)
            {
                _windSound.Stop();
                return;
            }
            else if (!_windSound.isPlaying)
            {
                _windSound.Play();
            }
            _windSound.panStereo = percentage;
            _windSound.volume = Mathf.Abs(percentage);
        }
        private void ResetTrail()
        {
            ClearTrail();
            _trail.enabled = false;
        }
        private void PlayAnimation() => _animator.Play("wind", 0, -1);
    }
}
