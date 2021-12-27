using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    class MinigameDrumAction : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private UnityEngine.UI.Image _image;
        public void Appear()
        {
            Activate();
            _animator.Play("Appear");
        }
        public void Hit()
        {
            _animator.Play("Hit");
        }
        public void Disappear()
        {
            _animator.Play("Disappear");
        }
        public void Activate() => _image.enabled = true;
        public void Deactivate() => _image.enabled = false;
    }
}
