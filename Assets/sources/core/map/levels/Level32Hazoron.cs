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
            Init();

            StatusEffectManager.AddRecoverAction((_) =>
            {
                _walking = true;
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
        }
        private void Update()
        {
            if (StatusEffectManager.IsOnStatusEffect) return;
            var hasTarget = DistanceCalculator.GetClosestForAttack() != null;
            if ((!IsAttacking && hasTarget) || ClassData.AttackMoveData.WasHitLastTime)
            {
                _walking = false;
                Attack();
            }
            else if (!hasTarget && IsAttacking)
            {
                StopAttacking(false);
                _walking = true;
                CharAnimator.Animate("walk");
            }
            if (_walking)
            {
                var pos = transform.position;
                pos.x = DistanceCalculator.GetSafeForwardPosition(pos.x + 0.5f * Time.deltaTime);
                transform.position = pos;
                DefaultWorldPosition = transform.position.x;
            }
        }
    }
}