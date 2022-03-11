using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    //Only general.
    public class Level33 : MonoBehaviour
    {
        [SerializeField]
        Character.Hazorons.Hazoron _luca;
        [SerializeField]
        Character.Hazorons.Hazoron _evo;
        [SerializeField]
        Character.Hazorons.Hazoron _vill;
        Character.Hazorons.Hazoron _aliveHazorons;
        int _aliveCount;
        void Start()
        {
            //prevent collection change
            foreach (var patapon in Character.Patapons.PataponsManager.Current.Patapons.ToArray())
            {
                if (!patapon.IsGeneral) patapon.BeEaten();
            }
            _vill.OnAfterDeath.AddListener(() =>
            {
                if (!_evo.IsDead) _evo.ChangeAttackStatus();
            });
            _evo.OnAfterDeath.AddListener(() =>
            {
                if (!_vill.IsDead)
                {
                    _vill.OnAfterDeath.AddListener(_luca.ChangeAttackStatus);
                }
                else
                {
                    _luca.ChangeAttackStatus();
                }
            });
            ConnectHP(_vill);
            ConnectHP(_evo);
            ConnectHP(_luca);
        }
        void ConnectHP(Character.Hazorons.Hazoron hazoron)
        {
            var statusBar = hazoron.GetComponentInChildren<Character.HealthDisplay>();
            hazoron.OnDamageTaken = new UnityEngine.Events.UnityEvent<float>();
            hazoron.OnDamageTaken.AddListener(statusBar.UpdateBar);
        }
    }
}