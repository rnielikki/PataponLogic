using PataRoad.Core.Character;
using PataRoad.Core.Character.Class;
using PataRoad.Core.Character.Hazorons;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level32Hazoron : Hazoron
    {
        bool _walking;
        private void Awake()
        {
            Charged = _charged;
            OnFever = true;
            DefaultWorldPosition = transform.position.x;
            Init();
            Stat = _data.Stat;
            DistanceCalculator = _isDarkOne
                ? DistanceCalculator.GetNonPataHazoDistanceCalculator(this)
                : DistanceCalculator.GetHazoronDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<DistanceManager>();
            DistanceManager.DistanceCalculator = DistanceCalculator;

            StatusEffectManager.AddRecoverAction((_) =>
            {
                _walking = false;
                IsAttacking = false;
                ClassData.AttackMoveData.WasHitLastTime = false;
                if (IsFlyingUnit)
                {
                    (ClassData as ToriClassData)?.FlyUp();
                }
                if (DistanceCalculator.GetClosestForAttack() != null)
                {
                    Attack();
                }
            });
            _attackTypeIndex = (_data as HazoronData).AttackTypeIndex;
        }
        private void Update()
        {
            if (StatusEffectManager.IsOnStatusEffect) return;
            var pos = transform.position;
            pos.x = DistanceCalculator.GetSafeForwardPosition(pos.x + 0.1f * Time.deltaTime);
            transform.position = pos;
            DefaultWorldPosition = transform.position.x;
            var hasTarget = DistanceCalculator.GetClosestForAttack() != null;
            if ((!IsAttacking && hasTarget) || ClassData.AttackMoveData.WasHitLastTime)
            {
                _walking = false;
                Attack();
            }
            else if (!hasTarget && !_walking)
            {
                StopAttacking(false);
                _walking = true;
                CharAnimator.Animate("walk");
            }
        }
    }
}