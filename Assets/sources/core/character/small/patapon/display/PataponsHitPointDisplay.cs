using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Core.Character.Patapon.Display
{
    /// <summary>
    /// Updates Patapons' hit point status PER GROUP.
    /// </summary>
    /// <note>This currently DOESN'T update HEAL status.</note>
    public class PataponsHitPointDisplay : MonoBehaviour
    {
        private static GameObject _displayTemplate;
        private static Transform _displayParent;

        private Image _image;
        private RectTransform _generalCurrent; //status bar of general
        private RectTransform _armyMinCurrent; //status bar of army min value

        private Image _generalImage;
        private Image _armyMinImage;

        private static Color _colorFull = new Color(0.23f, 0.97f, 0);
        private static Color _colorZero = new Color(0.98f, 0.19f, 0.19f);

        private float _currentMin = 1;

        void Awake()
        {
            _image = transform.Find("Image").GetComponent<Image>();
            _generalCurrent = transform.Find("General/GeneralCurrent").GetComponent<RectTransform>();
            _armyMinCurrent = transform.Find("ArmyMin/ArmyMinCurrent").GetComponent<RectTransform>();

            _generalImage = _generalCurrent.GetComponent<Image>();
            _armyMinImage = _armyMinCurrent.GetComponent<Image>();
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
            instance.UpdateGeneralStatus(group.General.GetComponent<Patapon>());
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
                UpdateStatus(patapon);
            }
        }
        private void UpdateGeneralStatus(Patapon patapon)
        {
            UpdateBar(GetCurrentHitPointPercent(patapon), true);
        }
        private void UpdateStatus(Patapon patapon)
        {
            var current = GetCurrentHitPointPercent(patapon);
            if (current < _currentMin)
            {
                UpdateBar(current);
                _currentMin = current;
            }
        }
        private float GetCurrentHitPointPercent(Patapon patapon) => Mathf.Clamp01((float)patapon.CurrentHitPoint / patapon.Stat.HitPoint);
        private void UpdateBar(float percent, bool general = false)
        {
            RectTransform bar;
            Image image;
            if (general)
            {
                bar = _generalCurrent;
                image = _generalImage;
            }
            else
            {
                bar = _armyMinCurrent;
                image = _armyMinImage;
            }
            bar.anchorMax = new Vector2(percent, 1);
            image.color = Color.Lerp(_colorZero, _colorFull, percent);
        }
        /// <summary>
        /// Refreshes non-general patapon health bar.
        /// </summary>
        /// <param name="patapons"></param>
        public void Refresh(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            var alivePataponArmy = patapons.Where(p => !p.IsGeneral && p.CurrentHitPoint > 0);
            if (!alivePataponArmy.Any())
            {
                //update as "all dead" image
                UpdateBar(0);
            }
            else
            {
                UpdateBar(alivePataponArmy.Min(p => p.CurrentHitPoint / p.Stat.HitPoint));
            }
            //group.Patapons.R
        }
    }
}
