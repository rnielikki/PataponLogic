using PataRoad.Core.Character;
using UnityEngine;
using UnityEngine.UI;
using PataRoad.Common.Navigator;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class StatDisplay : MonoBehaviour
    {
        //looooooooooooong fields
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

        private const string _format = "G5";
        private const string _percentFormat = "p0";

        public void UpdateGroup(SpriteSelectable selectable)
        {
            var stat = new Stat();
            foreach (var ponData in selectable
                .GetComponentsInChildren<PataponData>())
            {
                stat.Add(ponData.Stat);
            }
            UpdateStat(stat);
        }
        public void UpdateIndividual(SpriteSelectable selectable)
        {
            UpdateStat(selectable.GetComponent<PataponData>().Stat);
        }
        private void UpdateStat(Stat stat)
        {
            _stamina.text = stat.HitPoint.ToString(_format);
            _damage.text = $"{stat.DamageMin.ToString(_format)} - {stat.DamageMax.ToString(_format)}";
            _defence.text = $"{stat.DefenceMin.ToString(_format)} - {stat.DefenceMax.ToString(_format)}";
            _attackSeconds.text = $"{stat.AttackSeconds.ToString(_format)}s ({Core.Rhythm.RhythmEnvironment.TurnSeconds / stat.AttackSeconds:G3}/cmd)";
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
        }
    }
}
