using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PataRoad.Core.Character.Patapons.Display
{
    /// <summary>
    /// Updates Patapons' hit point status PER GROUP.
    /// </summary>
    public class PataponsHitPointDisplay : MonoBehaviour
    {
        private static GameObject _displayTemplate;
        private static Transform _displayParent { get; set; }

        private RawImage _image;
        [SerializeField]
        private HealthDisplay _generalCurrentBar; //status bar of general
        [SerializeField]
        private HealthDisplay _armyMinCurrentBar; //status bar of army min value

        private PataponGroup _group;

        [SerializeField]
        private Color _bgOnGeneral;
        [SerializeField]
        private Color _bgOnNoGeneral;

        private Patapon _currentFocus;

        private float _currentMinArmyHealth = 1;
        private PataponStatusRenderer _renderer;

        private bool _isGeneralAlive = true;
        private Image _bg;

        private Text _text;

        void Awake()
        {

            _bg = transform.Find("Image").GetComponent<Image>();
            _bg.color = _bgOnGeneral;

            _image = GetComponentInChildren<RawImage>();

            _text = GetComponentInChildren<Text>();
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
            }
            if (_displayParent == null)
            {
                _displayParent = GameObject.FindGameObjectWithTag("Screen").transform.Find("PataponsStatus");
            }
            var instance = Instantiate(_displayTemplate, _displayParent).GetComponent<PataponsHitPointDisplay>();

            instance._currentFocus = group.General.GetComponent<Patapon>();
            instance._group = group;
            instance.AddCamera();
            instance.UpdateText();

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
                RefreshArmyStatus(patapon, current);
            }
        }
        private void RefreshArmyStatus(Patapon patapon, float ratio)
        {
            UpdateImageAndBar(patapon);
            _currentMinArmyHealth = ratio;
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
            void UpdateBar(float ratio)
            {
                if (general)
                {
                    _generalCurrentBar.UpdateBar(ratio);
                }
                else
                {
                    _armyMinCurrentBar.UpdateBar(ratio);
                }
            }
        }
        public void OnDead(Patapon deadPon, System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            if (deadPon.IsGeneral)
            {
                _isGeneralAlive = false;
                _bg.color = _bgOnNoGeneral;
                _generalCurrentBar.UpdateBar(0);
            }
            Refresh(patapons);
            UpdateText();
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
                    RefreshArmyStatus(targetPatapon, GetCurrentHitPointPercent(targetPatapon));
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
        //Make sure to set always right value, even in Coroutine. Patapons in group are max 4 so it shouldn't take long
        private void UpdateText() => _text.text = _group.Patapons.Count(pon => !pon.IsDead).ToString();
        private void Update()
        {
            if (_currentFocus.IsDead && _group != null)
            {
                Refresh(_group.Patapons);
                UpdateText();
            }
        }
        private void OnDestroy()
        {
            _displayParent = null;
        }
    }
}
