using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.UI;
using PataRoad.Common.Navigator;
using System.Linq;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField]
        Text _header;

        [SerializeField]
        EquipmentSummary _equipmentSummary;

        //looooooooooooong fields
        [Header("Stat fields")]
        [SerializeField]
        Text _stamina;
        [SerializeField]
        Text _damageMin;
        [SerializeField]
        Text _damageMax;
        [SerializeField]
        Text _defenceMin;
        [SerializeField]
        Text _defenceMax;
        [SerializeField]
        Text _attackSeconds;
        [SerializeField]
        Text _movementSpeed;
        [SerializeField]
        Text _mass;

        [SerializeField]
        Text _critical;
        [SerializeField]
        Text _criticalResistance;
        [SerializeField]
        Text _knockback;
        [SerializeField]
        Text _knockbackResistance;
        [SerializeField]
        Text _stagger;
        [SerializeField]
        Text _staggerResistance;
        [SerializeField]
        Text _fire;
        [SerializeField]
        Text _fireResistance;
        [SerializeField]
        Text _ice;
        [SerializeField]
        Text _iceResistance;
        [SerializeField]
        Text _sleep;
        [SerializeField]
        Text _sleepResistance;

        [Header("Comparer colourz")]
        [SerializeField]
        Color _positiveColor;
        [SerializeField]
        Color _negativeColor;
        Color _neutralColor;

        [Header("Backgrounds")]
        [SerializeField]
        Image _bg1;
        [SerializeField]
        Image _bg2;
        [SerializeField]
        Color _backgroundAsGroup;

        private const string _format = "G3";
        private const string _percentFormat = "p0";

        private bool _onGroup;
        private SpriteSelectable _current;
        private PataponData _lastData;

        [SerializeField]
        private Text _guideText;
        private string _selectButtonName;

        bool _hasSelectButton;
        private StatDisplayMap[] _displayMaps;
        private StatDisplayMap _massDisplay;

        private void Start()
        {
            _displayMaps = new StatDisplayMap[]
            {
                new StatDisplayMap(_stamina, _format).SetValueGetter((stat) => stat.HitPoint),
                new StatDisplayMap(_damageMin, _format).SetValueGetter((stat) => stat.DamageMin),
                new StatDisplayMap(_damageMax, _format).SetValueGetter((stat) => stat.DamageMax),
                new StatDisplayMap(_defenceMin, _format).SetValueGetter((stat) => stat.DefenceMin),
                new StatDisplayMap(_defenceMax, _format).SetValueGetter((stat) => stat.DefenceMax),
                new StatDisplayMap(_attackSeconds, _format).SetValueGetter((stat) => stat.AttackSeconds)
                    .SetFormatGetter((stat)=> $"{stat.AttackSeconds.ToString(_format)}s ({Core.Rhythm.RhythmEnvironment.TurnSeconds / stat.AttackSeconds:G2}/cmd)")
                    .SetNegativeIsBetter(),
                new StatDisplayMap(_movementSpeed, _format).SetValueGetter((stat) => stat.MovementSpeed)
                    .SetFormatGetter((stat)=>$"{stat.MovementSpeed.ToString(_format)}s ({(stat.MovementSpeed / 8).ToString(_percentFormat)})"),

                new StatDisplayMap(_critical, _percentFormat).SetValueGetter((stat) => stat.Critical),
                new StatDisplayMap(_criticalResistance, _percentFormat).SetValueGetter((stat) => stat.CriticalResistance),
                new StatDisplayMap(_knockback, _percentFormat).SetValueGetter((stat) => stat.Knockback),
                new StatDisplayMap(_knockbackResistance, _percentFormat).SetValueGetter((stat) => stat.KnockbackResistance),
                new StatDisplayMap(_stagger, _percentFormat).SetValueGetter((stat) => stat.Stagger),
                new StatDisplayMap(_staggerResistance, _percentFormat).SetValueGetter((stat) => stat.StaggerResistance),

                new StatDisplayMap(_fire, _percentFormat).SetValueGetter((stat) => stat.FireRate),
                new StatDisplayMap(_fireResistance, _percentFormat).SetValueGetter((stat) => stat.FireResistance),
                new StatDisplayMap(_ice, _percentFormat).SetValueGetter((stat) => stat.IceRate),
                new StatDisplayMap(_iceResistance, _percentFormat).SetValueGetter((stat) => stat.IceResistance),
                new StatDisplayMap(_sleep, _percentFormat).SetValueGetter((stat) => stat.SleepRate),
                new StatDisplayMap(_sleepResistance, _percentFormat).SetValueGetter((stat) => stat.SleepResistance),
            };
            _massDisplay = new StatDisplayMap(_mass, _format);

            _neutralColor = _stamina.color;
            foreach (var display in _displayMaps)
            {
                display.AssignColors(_positiveColor, _neutralColor, _negativeColor);
            }
            _massDisplay.AssignColors(_positiveColor, _neutralColor, _negativeColor);

            _hasSelectButton = Core.Global.GlobalData.GlobalInputActions.TryGetActionBindingName("UI/Select", out _selectButtonName);
            OnChangedToGroup();
        }
        /// <summary>
        /// Show group average stat on group window.
        /// </summary>
        /// <param name="selectable">The selected Group.</param>
        public void UpdateGroup(SpriteSelectable selectable)
        {
            var data = selectable.GetComponentsInChildren<PataponData>();
            if (data.Length == 0)
            {
                Empty();
                return;
            }
            var stat = Stat.GetMidValue(data.Select(data => data.Stat));

            _header.text = $"{data[0].Type} - Average stat";
            if (!_onGroup)
            {
                OnChangedToGroup();
            }
            UpdateStat(stat, data.Average(d => d.Rigidbody.mass));
            _current = selectable;
            _lastData = null;
        }
        /// <summary>
        /// Show stat on class selection window.
        /// </summary>
        /// <param name="info">Information that provided in class selection window.</param>
        public void UpdateGroup(ClassSelectionInfo info)
        {
            if (info == null) Empty();
            else UpdateStat(info.StatAverage, info.MassAverage);
        }

        public void UpdateIndividual(SpriteSelectable selectable)
        {
            var pataponData = selectable.GetComponent<PataponData>();
            _header.text = $"{pataponData.Type}, {(pataponData.IsGeneral ? pataponData.GeneralName + " (General)" : pataponData.IndexInGroup.ToString())}";
            if (_onGroup)
            {
                OnChangedToIndividual();
            }
            UpdateStat(pataponData.Stat, pataponData.Rigidbody.mass);
            _current = selectable;
            _lastData = pataponData;
        }
        /// <summary>
        /// Empty the stat. Expected this when empty group is choosen.
        /// </summary>
        public void Empty()
        {
            _header.text = "Empty squad";

            foreach (var display in _displayMaps) display.SetText("");
            _massDisplay.SetText("");
        }
        /// <summary>
        /// Refresh stat after optimization.
        /// </summary>
        /// <param name="isFullOptimization"><c>true</c> if all groups are optimized. <c>false</c> if only one group is optimized.</param>
        public void RefreshStat(bool isFullOptimization)
        {
            if (isFullOptimization)
            {
                UpdateGroup(_current);
            }
            else
            {
                UpdateIndividual(_current);
            }
        }
        public void CompareStat(Core.Items.IItem item)
        {
            if (_lastData == null || item == null || item.ItemType != Core.Items.ItemType.Equipment) return;
            var equipmentData = item as Core.Items.EquipmentData;
            if (equipmentData != null && equipmentData.Type != Core.Character.Equipments.EquipmentType.Rarepon)
            {
                var oldEquipment = _lastData.EquipmentManager.GetEquipmentData(equipmentData.Type);
                var oldEquipmentStat = oldEquipment?.Stat ?? new Stat();
                var newEquipmentStat = equipmentData.Stat;
                foreach (var display in _displayMaps)
                {
                    display.CompareOneByOne(_lastData.Stat, oldEquipmentStat, newEquipmentStat);
                }
                _massDisplay.CompareValue(_lastData.Rigidbody.mass, oldEquipment?.Mass ?? 0, equipmentData.Mass);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
        public void ResetAllComparisonColors()
        {
            foreach (var display in _displayMaps)
            {
                display.ResetColor();
            }
            _massDisplay.ResetColor();
        }

        private void OnChangedToGroup()
        {
            _bg1.color = _backgroundAsGroup;
            _bg2.color = _backgroundAsGroup;
            _equipmentSummary.gameObject.SetActive(false);
            if (_hasSelectButton) _guideText.text = $"{_selectButtonName} to see/change attack type";

            _onGroup = true;
        }

        private void OnChangedToIndividual()
        {
            _bg1.color = Color.white;
            _bg2.color = Color.white;
            _equipmentSummary.gameObject.SetActive(true);
            if (_hasSelectButton) _guideText.text = $"{_selectButtonName} to see attack type resistance";

            _onGroup = false;
        }
        private void UpdateStat(Stat stat, float mass)
        {
            foreach (var statDisplay in _displayMaps)
            {
                statDisplay.UpdateText(stat);
            }
            _massDisplay.SetText(mass.ToString(_format));
        }
    }
}
