using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Character.Patapons.Display
{
    /// <summary>
    /// Updates Patapons' hit point status PER GROUP.
    /// </summary>
    /// <note>This currently DOESN'T update HEAL status.</note>
    public class PataponsHitPointDisplay : MonoBehaviour
    {
        private static GameObject _displayTemplate;
        private static Transform _displayParent;

        private RawImage _image;
        private RectTransform _generalCurrentBar; //status bar of general
        private RectTransform _armyMinCurrentBar; //status bar of army min value

        private Image _generalImage;
        private Image _armyMinImage;

        [SerializeField]
        private Color _bgOnGeneral;
        [SerializeField]
        private Color _bgOnNoGeneral;
        [SerializeField]
        private Gradient _colorOverHealth;

        private Patapon _currentFocus;

        private float _currentMinArmyHealth = 1;
        private PataponStatusRenderer _renderer;

        private bool _isGeneralAlive = true;
        private Image _bg;

        void Awake()
        {
            _bg = transform.Find("Image").GetComponent<Image>();
            _bg.color = _bgOnGeneral;

            _image = GetComponentInChildren<RawImage>();
            _generalCurrentBar = transform.Find("General/GeneralCurrent").GetComponent<RectTransform>();
            _armyMinCurrentBar = transform.Find("ArmyMin/ArmyMinCurrent").GetComponent<RectTransform>();

            var colorFull = _colorOverHealth.Evaluate(1);
            _generalCurrentBar.GetComponent<Image>().color = colorFull;
            _armyMinCurrentBar.GetComponent<Image>().color = colorFull;

            _generalImage = _generalCurrentBar.GetComponent<Image>();
            _armyMinImage = _armyMinCurrentBar.GetComponent<Image>();
        }
        /// <summary>
        /// Adds hitpoint status to the display for one group.
        /// </summary>
        /// <returns><see cref="PataponsHitPointDisplay"/> that attached to the instantiated object.</returns>
        public static PataponsHitPointDisplay Add(PataponGroup group)
        {
            if (_displayTemplate == null)
            {
                _displayTemplate = Resources.Load<GameObject>("Characters/Patapons/Display/PonStatus");
                _displayParent = GameObject.FindGameObjectWithTag("Screen").transform.Find("PataponsStatus");
            }
            var instance = Instantiate(_displayTemplate, _displayParent).GetComponent<PataponsHitPointDisplay>();

            instance._currentFocus = group.General.GetComponent<Patapon>();
            instance.AddCamera();

            return instance;
        }
        public void UpdateHitPoint(Patapon patapon)
        {
            if (patapon.IsGeneral)
            {
                UpdateGeneralStatus(patapon);
            }
            else
            {
                UpdateArmyStatus(patapon);
            }
        }
        private void UpdateGeneralStatus(Patapon patapon)
        {
            UpdateImageAndBar(patapon, true);
        }
        private void UpdateArmyStatus(Patapon patapon)
        {
            var current = GetCurrentHitPointPercent(patapon);
            if (current < _currentMinArmyHealth)
            {
                UpdateImageAndBar(patapon);
                _currentMinArmyHealth = current;
            }
        }
        private float GetCurrentHitPointPercent(Patapon patapon) => Mathf.Clamp01((float)patapon.CurrentHitPoint / patapon.Stat.HitPoint);

        private void UpdateImageAndBar(Patapon patapon, bool general = false)
        {
            if (patapon == null)
            {
                UpdateBar(0);
                return;
            }
            var per = GetCurrentHitPointPercent(patapon);

            UpdateImage();
            UpdateBar(per);

            void UpdateImage()
            {
                if (!_isGeneralAlive && patapon != _currentFocus)
                {
                    _currentFocus = patapon;
                    _renderer.SetTarget(patapon);
                }
            }
            void UpdateBar(float percent)
            {
                RectTransform bar;
                Image image;
                if (general)
                {
                    bar = _generalCurrentBar;
                    image = _generalImage;
                }
                else
                {
                    bar = _armyMinCurrentBar;
                    image = _armyMinImage;
                }
                bar.anchorMax = new Vector2(percent, 1);
                image.color = _colorOverHealth.Evaluate(percent);
            }
        }
        public void OnDead(Patapon deadPon, System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            if (deadPon.IsGeneral)
            {
                _isGeneralAlive = false;
                _bg.color = _bgOnNoGeneral;
            }
            Refresh(patapons);
        }

        /// <summary>
        /// Refreshes patapon health bar. This can be called when any Patapon is healed.
        /// </summary>
        /// <param name="patapons"></param>
        public void Refresh(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            if (_renderer == null) return;
            var alivePataponArmy = patapons.Where(p => !p.IsGeneral && p.CurrentHitPoint > 0);
            bool aliveAnyArmy = alivePataponArmy.Any();
            if (!_isGeneralAlive && !aliveAnyArmy)
            {
                //update as "all dead" image
                UpdateImageAndBar(null);
                Destroy(_renderer.gameObject);
            }
            else
            {
                if (_isGeneralAlive) UpdateGeneralStatus(patapons.First(p => p.IsGeneral));
                if (aliveAnyArmy)
                {
                    //MinBy is .NET 6 RC 1... Which means far away from Unity :/
                    var targetPatapon = alivePataponArmy.Aggregate((p1, p2) =>
                        (GetCurrentHitPointPercent(p1) < GetCurrentHitPointPercent(p2)) ? p1 : p2
                        );
                    _currentMinArmyHealth = 2;
                    _currentFocus = null;
                    UpdateArmyStatus(targetPatapon);
                }
            }
        }

        private void AddCamera()
        {
            var renderTexture = new RenderTexture(50, 50, 0);
            _image.texture = renderTexture;

            var camRes = Resources.Load<GameObject>("Characters/Patapons/Display/Camera");
            var camObject = Instantiate(camRes, _currentFocus.transform.parent);
            _renderer = camObject.GetComponent<PataponStatusRenderer>();
            _renderer.Init(_currentFocus, renderTexture);
        }
    }
}
