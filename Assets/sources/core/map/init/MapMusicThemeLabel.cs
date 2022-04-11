using UnityEngine;

namespace PataRoad.Core.Map
{
    public class MapMusicThemeLabel : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI _text;

        private bool _fading;
        private int _counter = 16;
        internal void Init(string name)
        {
            _text.text = $"~~ Now playing: {name} ~~";
        }
        private void Start()
        {
            Rhythm.RhythmTimer.Current.OnTime.AddListener(CountDown);
        }
        private void CountDown()
        {
            _counter--;
            if (_counter == 0)
            {
                Rhythm.RhythmTimer.Current.OnTime.RemoveListener(CountDown);
                _fading = true;
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (_fading)
            {
                var clr = _text.color;
                clr.a = Mathf.Clamp01(clr.a - Time.deltaTime * 2);
                if (clr.a == 0) gameObject.SetActive(false);
                else _text.color = clr;
            }
        }
    }
}