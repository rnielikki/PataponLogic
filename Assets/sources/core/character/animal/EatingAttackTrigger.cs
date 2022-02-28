using UnityEngine;

namespace PataRoad.Core.Character.Animal
{
    class EatingAttackTrigger : MonoBehaviour
    {
        bool _ate;
        public void ResetEater() => _ate = false;
        [SerializeField]
        AudioClip _eatenSound;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_ate || collider.gameObject.tag != "SmallCharacter") return;
            var patapon = collider.GetComponentInParent<Patapons.Patapon>();
            if (patapon != null)
            {
                patapon.BeEaten();
                GameSound.SpeakManager.Current.Play(_eatenSound);
                _ate = true;
            }
        }
    }
}
