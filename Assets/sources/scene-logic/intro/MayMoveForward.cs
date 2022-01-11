using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.SceneLogic.Intro
{
    class MayMoveForward : MonoBehaviour
    {
        bool _started;
        bool _startedTransition;
        [SerializeField]
        Camera _camera;
        [SerializeField]
        Image _transition;
        private float _opacity;
        public void GoForward()
        {
            _camera.GetComponent<Animator>().enabled = false;
            _started = true;
            StartCoroutine(WaitAndStartTransition());
            System.Collections.IEnumerator WaitAndStartTransition()
            {
                yield return new WaitForSeconds(3);
                StartTransition();
            }
        }
        public void StartTransition()
        {
            _transition.transform.parent.gameObject.SetActive(true);
            _startedTransition = true;
        }
        private void Update()
        {
            if (_started)
            {
                transform.position += Time.deltaTime * transform.forward * 10;
                _camera.transform.Translate(0, Time.deltaTime * 2, Time.deltaTime * -20);
            }
            if (_startedTransition)
            {
                _opacity += Mathf.Clamp01(Time.deltaTime * 2);
                _transition.color = new Color(1, 1, 1, _opacity);
                if (_opacity >= 1)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
                }
            }
        }

    }
}
