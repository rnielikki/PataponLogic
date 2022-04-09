using UnityEngine;

namespace PataRoad.Core.Map.Weather
{
    public class Clouds : MonoBehaviour
    {
        [SerializeField]
        Animator _animator;
        public bool IsCloudOn { get; private set; }
        public void Show()
        {
            if (IsCloudOn) return;
            IsCloudOn = true;
            gameObject.SetActive(true);
            _animator.SetBool("nocloud", false);
            _animator.Play("cloud-in");
        }
        public void StartHiding()
        {
            if (!IsCloudOn) return;
            IsCloudOn = false;
            _animator.SetBool("nocloud", true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}