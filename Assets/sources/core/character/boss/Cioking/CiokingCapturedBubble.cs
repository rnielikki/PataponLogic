using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class CiokingCapturedBubble : MonoBehaviour
    {
        bool _captured;
        private Patapons.Patapon _patapon;
        internal void Capture(Patapons.Patapon patapon, Vector3 position)
        {
            _captured = true;
            _patapon = patapon;
            patapon.BeTaken();
            transform.position = position;
            patapon.transform.parent = transform;
            patapon.transform.localPosition = Vector3.zero;
            foreach (var collider in patapon.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }
            gameObject.SetActive(true);
        }
        private void Update()
        {
            if (_captured)
            {
                transform.Translate(0, Time.deltaTime, 0);
                if (transform.position.y >= 5)
                {
                    GameSound.SpeakManager.Current.Play(_patapon.Sounds.OnDead);
                    _patapon.EnsureDeath();
                    _captured = false;
                    _patapon = null;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}