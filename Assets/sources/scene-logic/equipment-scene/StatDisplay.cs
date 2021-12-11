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
        Text _damage;
        [SerializeField]
        Text _defence;
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

        [SerializeField]
        private Text _guideText;
        private string _selectButtonName;

        bool _hasSelectButton;

        private void Start()
        {
            _hasSelectButton = Core.Global.GlobalData.TryGetActionBindingName("UI/Select", out _selectButtonName);
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
        }
        /// <summary>
        /// Empty the stat. Expected this when empty group is choosen.
        /// </summary>
        public void Empty()
        {
            _header.text = "Empty squad";

            _stamina.text = "-";
            _damage.text = "-";
            _defence.text = "-";
            _attackSeconds.text = "-";
            _movementSpeed.text = "-";

            _critical.text = "-";
            _criticalResistance.text = "-";
            _knockback.text = "-";
            _knockbackResistance.text = "-";
            _stagger.text = "-";
            _staggerResistance.text = "-";

            _fire.text = "-";
            _fireResistance.text = "-";
            _ice.text = "-";
            _iceResistance.text = "-";
            _sleep.text = "-";
            _sleepResistance.text = "-";

            _mass.text = "-";
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
            _stamina.text = stat.HitPoint.ToString(_format);
            _damage.text = $"{stat.DamageMin.ToString(_format)} - {stat.DamageMax.ToString(_format)}";
            _defence.text = $"{stat.DefenceMin.ToString(_format)} - {stat.DefenceMax.ToString(_format)}";
            _attackSeconds.text = $"{stat.AttackSeconds.ToString(_format)}s ({Core.Rhythm.RhythmEnvironment.TurnSeconds / stat.AttackSeconds:G2}/cmd)";
            _movementSpeed.text = $"{stat.MovementSpeed.ToString(_format)}s ({(stat.MovementSpeed / 8).ToString(_percentFormat)})";

            _critical.text = stat.Critical.ToString(_percentFormat);
            _criticalResistance.text = stat.CriticalResistance.ToString(_percentFormat);
            _knockback.text = stat.Knockback.ToString(_percentFormat);
            _knockbackResistance.text = stat.KnockbackResistance.ToString(_percentFormat);
            _stagger.text = stat.Stagger.ToString(_percentFormat);
            _staggerResistance.text = stat.StaggerResistance.ToString(_percentFormat);

            _fire.text = stat.FireRate.ToString(_percentFormat);
            _fireResistance.text = stat.FireResistance.ToString(_percentFormat);
            _ice.text = stat.IceRate.ToString(_percentFormat);
            _iceResistance.text = stat.IceResistance.ToString(_percentFormat);
            _sleep.text = stat.SleepRate.ToString(_percentFormat);
            _sleepResistance.text = stat.SleepResistance.ToString(_percentFormat);

            _mass.text = mass.ToString(_format);
        }
    }
}
