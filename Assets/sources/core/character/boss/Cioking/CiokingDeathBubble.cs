using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class CiokingDeathBubble : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private bool _onGround;
        private float _disapperTimer;

        private Vector3 _direction;

        CiokingCapturedBubble _caputredBubble;

        public void Init(Vector3 direction)
        {
            if (_rigidbody != null) return;
            _direction = direction;
            _rigidbody = GetComponent<Rigidbody2D>();
            _caputredBubble = GetComponentInChildren<CiokingCapturedBubble>(true);
            _caputredBubble.transform.parent = transform.root;
        }
        public void Throw(float force)
        {
            _rigidbody.WakeUp();
            _rigidbody.velocity = Vector2.zero;
            transform.localPosition = Vector3.zero;
            _rigidbody.AddForce(force * _direction);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                StopBubble();
                _disapperTimer = 0.2f;
                _onGround = true;
            }
            else if (collision.gameObject.CompareTag("SmallCharacter"))
            {
                var patapon = collision.gameObject.GetComponentInParent<Patapons.Patapon>();
                if (patapon == null) return;
                StopBubble();
                _caputredBubble.Capture(patapon, transform.position);
                HideBubble();
            }
        }
        private void StopBubble()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.Sleep();
        }
        private void HideBubble()
        {
            gameObject.SetActive(false);
            _onGround = false;
        }
        private void Update()
        {
            if (_onGround)
            {
                _disapperTimer -= Time.deltaTime;
                if (_disapperTimer <= 0)
                {
                    HideBubble();
                }
            }
        }
    }
}