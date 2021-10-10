using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Rhythm.Display
{
    /// <summary>
    /// Displays combo and fever status worm.
    /// <note>If you want better effect like image or border, use material with shader.</note>
    /// </summary>
    public class ComboStatusDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Image _eyesImage;
        [SerializeField]
        Sprite _eyesNoFever;
        [SerializeField]
        Sprite _eyesFever;
        [SerializeField]
        Color _startColorNoFever;
        [SerializeField]
        Color _endColorNoFever;
        [SerializeField]
        Color _startColorBeforeFever;
        [SerializeField]
        Color _endColorBeforeFever;
        [SerializeField]
        Color _startColorFever;
        [SerializeField]
        Color _endColorFever;

        RectTransform _eyesPos;
        Vector3 _eyesInitialPosition;
        Text _text;
        Text _number;
        Animator _animator;
        Color _currentStartColor, _currentEndColor;

        LineRenderer _wormBody;
        int _wormMaxLength;
        /// <summary>
        /// worm animation parts. The bigger the value, the finer the animation is. Still don't set toooo much for performance.
        /// </summary>
        const int animationParts = 16;

        //------------------------ for animation on fixed time!
        //(because this is *special* animation with Line renderer --)
        int _wormAnimationCounter;
        bool _wormAnimationCounting;

        int _comboAnimHash, _idleAnimHash, _feverAnimHash, _feverAppearAnimHash;
        void Awake()
        {
            _wormBody = transform.Find("Image").GetComponent<LineRenderer>();
            //Trick here: The thickness of line is world space, but position isn't!
            _wormBody.useWorldSpace = false;
            _wormMaxLength = (int)transform.Find("Image").GetComponent<RectTransform>().rect.width;

            var eyes = transform.Find("Image/Face");
            _eyesImage = eyes.GetComponent<Image>();
            _eyesPos = eyes.GetComponent<RectTransform>();
            _eyesInitialPosition = _eyesPos.localPosition;

            _text = transform.Find("Combo").GetComponent<Text>();
            _number = transform.Find("Combo/Number").GetComponent<Text>();
            _animator = GetComponent<Animator>();

            _comboAnimHash = Animator.StringToHash("Start-Combo");
            _feverAnimHash = Animator.StringToHash("Fever");
            _feverAppearAnimHash = Animator.StringToHash("Fever-Appear");
            _idleAnimHash = Animator.StringToHash("Idle");
            Hide();
        }
        public void Show(Command.RhythmComboModel comboInfo)
        {
            if (comboInfo.ComboCount < 2) return;
            if (!gameObject.activeSelf) SetComboText();
            if (comboInfo.hasFeverChance)
            {
                _eyesImage.sprite = _eyesFever;
                _currentStartColor = _startColorBeforeFever;
                _currentEndColor = _endColorBeforeFever;
                SetBounceAnimation();
            }
            else
            {
                _currentStartColor = _startColorNoFever;
                _currentEndColor = _endColorNoFever;
                _eyesImage.sprite = _eyesNoFever;
                StopAllCoroutines();
                ResetBounceAnimation();
            }

            _number.text = comboInfo.ComboCount.ToString();
            _animator.Play(_comboAnimHash);
        }
        public void ShowFever()
        {
            _currentStartColor = _startColorFever;
            _currentEndColor = _endColorFever;
            _number.enabled = false;
            _text.text = "FEVER!!";

            _animator.Play(_feverAnimHash);

            StopAllCoroutines();
            StartCoroutine(PlayFeverEnterAnimation());
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
            _animator.Play(_idleAnimHash);
            ResetBounceAnimation();
            gameObject.SetActive(false);
            StopAnimationCounter();
        }
        public void DisplayCommandScore(Command.RhythmCommandModel model)
        {
            if (!gameObject.activeSelf) return;
            SetColor(_currentStartColor, _currentEndColor, model.Percentage);
        }
        private void SetColor(Color startColor, Color endColor, float percentage) //percentage between bad-perfect, 0-1.
        {
            var gradientColors = new GradientColorKey[4];
            SetGradient(ref gradientColors[0], startColor, 0);
            SetGradient(ref gradientColors[1], startColor, percentage);
            SetGradient(ref gradientColors[2], endColor, percentage);
            SetGradient(ref gradientColors[3], endColor, 1);
            var gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;
            gradient.SetKeys(gradientColors, new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            _wormBody.colorGradient = gradient;

            void SetGradient(ref GradientColorKey key, Color color, float offset)
            {
                key.color = color;
                key.time = offset;
            }
        }
        private void ResetBounceAnimation(bool resetColor = true)
        {
            _eyesPos.localPosition = _eyesInitialPosition;
            _wormBody.positionCount = 2;
            if (resetColor) SetColor(Color.black, Color.black, 0);
            _wormBody.SetPosition(0, new Vector3(0, 0, 0));
            _wormBody.SetPosition(1, new Vector3(_wormMaxLength, 0, 0));
        }

        private void SetBounceAnimation()
        {
            if (_wormBody.positionCount > 2) return; //already playing
            _wormBody.positionCount = animationParts;
            StartCoroutine(PlayBounceAnimation());
        }
        //I don't know math so if there's better way please fix this...
        //https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
        private IEnumerator PlayFeverEnterAnimation()
        {
            _wormBody.positionCount = animationParts;
            float startOffset = -1;
            float endOffset = 1.25f;

            StartAnimationCounter();
            while (gameObject.activeSelf && startOffset < endOffset)
            {
                //If you're here for this kind of logic, I'd say this won't work if there are too many delay on frame (small FPS).
                //startOffset += interval * (freq + RhythmTimer.Frequency - _savedFreq) % RhythmTimer.Frequency
                startOffset = 0.01f * _wormAnimationCounter;

                Draw(startOffset);
                yield return new WaitForEndOfFrame();
            }
            StopAnimationCounter();
            Command.TurnCounter.OnNextTurn.AddListener(() =>
            {
                ResetBounceAnimation(false);
                _eyesImage.sprite = _eyesFever;
                SetBounceAnimation();
                _animator.Play(_feverAppearAnimHash);
            });
            void Draw(float startOffset)
            {
                const int wormHeight = 100;
                float t = startOffset;
                Vector3 point0 = Vector3.zero;
                Vector3 point1 = new Vector3(_wormMaxLength, -wormHeight / 2, 0) + Vector3.zero;
                Vector3 point2 = new Vector3(0, -wormHeight, 0);
                Vector3 B = Vector3.zero;
                for (int i = 0; i < animationParts; i++)
                {
                    B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                    _wormBody.SetPosition(i, B);
                    t += (1 / (float)_wormBody.positionCount);
                }
                _eyesPos.localPosition = B;
            }
        }
        private IEnumerator PlayBounceAnimation()
        {
            int maxOffset = 10;
            float yOffset = maxOffset;
            float interval = 0.5f;
            bool rising = true;
            Vector3 vectorInterval = Vector3.up * interval * 0.5f;
            StartAnimationCounter();
            while (gameObject.activeSelf)
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

                var offset = _wormAnimationCounter;
                if (rising)
                {
                    yOffset += interval * offset;
                    _eyesPos.localPosition += vectorInterval * offset;
                }
                else
                {
                    yOffset -= interval * offset;
                    _eyesPos.localPosition -= vectorInterval * offset;
                }
                _wormAnimationCounter = 0;
                yield return new WaitForEndOfFrame();
            }
            StopAnimationCounter();
        }
        private void StartAnimationCounter()
        {
            _wormAnimationCounting = true;
            _wormAnimationCounter = 0;
        }
        //PLEASE CALL THIS IF YOU WANT TO FIX THE WORM BODY MOVING LOGIC...
        private void StopAnimationCounter()
        {
            _wormAnimationCounting = false;
            _wormAnimationCounter = 0;
        }
        private void FixedUpdate()
        {
            if (_wormAnimationCounting)
            {
                _wormAnimationCounter++;
            }
        }
        private void OnDestroy()
        {
            Hide();
        }
    }
}
