using UnityEngine;

namespace PataRoad.Core.Character
{
    class MeleeDistanceCalculator : DistanceCalculator
    {
        internal MeleeDistanceCalculator(ICharacter character, float sight, int layerMask) : base(character, sight, layerMask)
        {
        }

        protected override Vector2? GetClosest(Vector2 castPoint)//bidirectional
        {
            var raycast = Physics2D.Raycast(castPoint, _direction, _sight, LayerMask);
            if (raycast.collider == null) return null;
            else return raycast.point;
        }
        public override float GetSafeForwardPosition(float input)
        {
            var raycast = Physics2D.Raycast(_target.transform.position, _direction, _sight, LayerMask);
            if (raycast.collider == null)
            {
                return input;
            }
            var xDir = _direction.x;
            if (xDir < 0)
            {
                return Mathf.Max(raycast.point.x, input);
            }
            else
            {
                return Mathf.Min(raycast.point.x, input);
            }
        }

    }
}
