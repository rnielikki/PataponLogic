using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Distance manager for one Patapon. Without this, Patapon won't move when attack or defence etc. Attach same object as Patapon script component. USE WITH ANIMATION EVENT.
    /// </summary>
    /// <remarks>This is NON-CONTINUOS MOVE in turn. When it arrives, it ends moving in same turn. for continuos move, see <see cref="AttackMoveController"/>.</remarks>
    /// <note>All values are relative to <see cref="PataponManager"/>Position, with own offset index.</note>
    public class PataponDistanceManager : DistanceManager
    {
        public override float DefaultWorldPosition => _defaultPosition.x + _pataponGroupTransform.position.x;

        private Transform _pataponGroupTransform;
        //All are moved from this
        private Transform _pataponsManagerTransform;

        /// <summary>
        /// Returns current x value of <see cref="PataponsManager"/>.
        /// </summary>
        public float Front => _pataponsManagerTransform.position.x;
        private Patapon _patapon;
        protected override Vector2 _parentPosition => _pataponGroupTransform.position;

        internal void Start()
        {
            Init();
            _patapon = _smallCharacter as Patapon;
            _pataponsManagerTransform = GetComponentInParent<PataponsManager>().transform;
            _pataponGroupTransform = GetComponentInParent<PataponGroup>().transform;
            _defaultPosition = transform.position - _pataponGroupTransform.position;
            Offset = _patapon.IndexInGroup * PataponEnvironment.AttackDistanceBetweenPatapons;
        }

        /// <summary>
        /// Move to specific position (*RELATIVE TO WHOLE PATAPON MANAGER) with certain speed.
        /// </summary>
        /// <param name="positionOffset">Position, relative from Patapon Manager 0 position. + is forward, - is backward.</param>
        /// <param name="velocity">Speed, how much will move per second. ALWAYS +.</param>
        public override void MoveTo(float positionOffset, float velocity, bool ignoreSafeDistance = false)
        {
            float x = _pataponsManagerTransform.position.x + positionOffset;
            var hit = DistanceCalculator.GetClosest();
            if (hit != null && !ignoreSafeDistance)
            {
                x = Mathf.Min(
                    hit.Value.x,
                    x
                );
            }
            MoveWithTargetPosition(x, velocity, ignoreSafeDistance);
        }
        /// <summary>
        /// Changes DEFAULT POSITION to front, when other Patapon dies on the front in the same group.
        /// </summary>
        public void UpdateDefaultPosition()
        {
            _defaultPosition = _patapon.IndexInGroup * PataponEnvironment.PataponIdleDistance * Vector2.left;
        }
    }
}
