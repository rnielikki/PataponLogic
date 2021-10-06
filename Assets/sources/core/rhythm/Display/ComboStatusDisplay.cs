using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Rhythm.Display
{
    public class ComboStatusDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Image _eyesImage;
        [SerializeField]
        Sprite _eyesNoFever;
        [SerializeField]
        Sprite _eyesFever;
        RectTransform _eyesPos;
        Vector3 _eyesInitialPosition;
        Text _text;
        Text _number;
        ParticleSystem _particle;
        Animator _animator;

        LineRenderer _wormBody;
        int _wormMaxLength;
        /// <summary>
        /// worm animation parts. The bigger the value, the finer the animation is. Still don't set toooo much for performance.
        /// </summary>
        const int animationParts = 16;

        int _comboAnimHash, _feverAnimHash;
        void Awake()
        {
            _wormBody = transform.Find("Image").GetComponent<LineRenderer>();
            _wormMaxLength = (int)transform.Find("Image").GetComponent<RectTransform>().rect.width;

            var eyes = transform.Find("Image/Face");
            _eyesImage = eyes.GetComponent<Image>();
            _eyesPos = eyes.GetComponent<RectTransform>();
            _eyesInitialPosition = _eyesPos.localPosition;

            _text = transform.Find("Combo").GetComponent<Text>();
            _number = transform.Find("Combo/Number").GetComponent<Text>();
            _particle = GetComponentInChildren<ParticleSystem>();
            _animator = GetComponent<Animator>();

            _comboAnimHash = Animator.StringToHash("Start-Combo");
            _feverAnimHash = Animator.StringToHash("Fever");
            Hide();
        }
        public void Show(Command.RhythmComboModel comboInfo)
        {
            if (comboInfo.ComboCount < 2) return;
            if (!gameObject.activeSelf) SetComboText();
            if (!enabled) enabled = true;
            if (comboInfo.hasFeverChance)
            {
                _eyesImage.sprite = _eyesFever;
                SetColor(Color.green, Color.blue);
                SetBounceAnimation();
            }
            else
            {
                _eyesImage.sprite = _eyesNoFever;
                StopAllCoroutines();
                ResetBounceAnimation();
            }

            _number.text = comboInfo.ComboCount.ToString();
            _animator.Play(_comboAnimHash);
        }
        public void ShowFever()
        {
            _eyesImage.sprite = _eyesFever;
            _number.enabled = false;
            _text.text = "FEVER!!";
            _animator.Play(_feverAnimHash);
            SetColor(Color.red, Color.yellow);
            SetBounceAnimation();
        }
        private void SetComboText()
        {
            _number.enabled = true;
            _text.text = "  COMBO!";
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            StopAllCoroutines();
            _number.enabled = false;
            _animator.StopPlayback();
            ResetBounceAnimation();
            gameObject.SetActive(false);
        }
        public void EffectOnPerfect(Command.RhythmCommandModel model)
        {
            if (model.PerfectCount == 4 && gameObject.activeSelf)
            {
                _particle.Play();
            }
        }
        private void SetColor(Color startColor, Color endColor)
        {
            _wormBody.startColor = startColor;
            _wormBody.endColor = endColor;
        }
        private void ResetBounceAnimation()
        {
            _eyesPos.localPosition = _eyesInitialPosition;
            _wormBody.positionCount = 2;
            SetColor(Color.black, Color.black);
            _wormBody.SetPosition(0, new Vector3(0, 0, 0));
            _wormBody.SetPosition(1, new Vector3(_wormMaxLength, 0, 0));
        }

        private void SetBounceAnimation()
        {
            _wormBody.positionCount = animationParts;
            StartCoroutine(PlayBounceAnimation());
        }
        private IEnumerator PlayBounceAnimation()
        {
            int maxOffset = 10;
            float yOffset = maxOffset;
            float interval = 0.5f;
            bool rising = true;
            Vector3 vectorInterval = Vector3.up * interval * 0.5f;
            while (true)
            {
                for (int i = 0; i < animationParts; i++)
                {
                    _wormBody.SetPosition(i, new Vector3(i * _wormMaxLength / animationParts, Mathf.Sin(((float)i / animationParts) * Mathf.PI * 5) * yOffset, 0));
                }
                if (yOffset >= maxOffset)
                {
                    rising = false;
                }
                else if (yOffset <= -maxOffset)
                {
                    rising = true;
                }

                if (rising)
                {
                    yOffset += interval;
                    _eyesPos.localPosition += vectorInterval;
                }
                else
                {
                    yOffset -= interval;
                    _eyesPos.localPosition -= vectorInterval;
                }
                yield return new WaitForFixedUpdate();
            }

        }
    }
}
