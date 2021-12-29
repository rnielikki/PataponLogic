using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    class MinigameDrumAction : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private UnityEngine.UI.Image _image;
        [SerializeField]
        private UnityEngine.UI.Image _statusImage;
        public void Appear()
        {
            _animator.SetBool("Cleared", false);
            Activate();
            _animator.Play("Appear");
        }
        public void Hit(float accuracy)
        {
            _animator.Play("Hit");
            _statusImage.enabled = true;
            _statusImage.transform.localScale = Vector3.one * (1 - accuracy);
        }
        public void Disappear()
        {
            _animator.Play("Disappear");
        }
        public void Activate() => _image.enabled = true;
        public void Deactivate() => _image.enabled = false;
        public void ResetStatus()
        {
            _animator.SetBool("Cleared", true);
        }
    }
}
