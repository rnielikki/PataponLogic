using UnityEngine;
using UnityEngine.UI;

namespace Core.Rhythm.Display
{
    public class ComboStatusDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Image _image;
        Image _face;
        [SerializeField]
        Sprite _eyesNoFever;
        [SerializeField]
        Sprite _eyesFever;
        Text _text;
        ParticleSystem _particle;
        void Awake()
        {
            _image = transform.Find("Image").GetComponent<Image>();
            _face = transform.Find("Face").GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            _particle = GetComponentInChildren<ParticleSystem>();
            Hide();
        }
        public void Show(System.ValueTuple<int, int> comboInfo)
        {
            (int combo, int chain) = comboInfo;
            if (combo < 2) return;
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            if (!enabled) enabled = true;
            if (chain > 0)
            {
                _image.color = Color.magenta;
                _face.sprite = _eyesFever;
            }
            else
            {
                _image.color = Color.black;
                _face.sprite = _eyesNoFever;
            }
            _text.text = combo + " COMBO!";
        }
        public void ShowFever()
        {
            _image.color = Color.red;
            _face.sprite = _eyesFever;
            _text.text = "FEVER!!";
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void EffectOnPerfect(Command.RhythmCommandModel model)
        {
            if (model.PerfectCount == 4 && gameObject.activeSelf)
            {
                _particle.Play();
            }
        }
        //For miracle
    }
}
