using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : MonoBehaviour
    {
        public PataponGeneral General { get; private set; }

        private System.Collections.Generic.List<Patapon> _patapons;
        public int Index { get; internal set; }

        private Display.PataponsHitPointDisplay _pataponsHitPointDisplay;
        public ClassType ClassType { get; internal set; }
        private float _marchiDistance;
        private LayerMask _layerMask;

        private PataponsManager _manager;

        internal void Init(PataponsManager manager)
        {
            if (_patapons != null) return;
            General = GetComponentInChildren<PataponGeneral>();
            _patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            _marchiDistance = _patapons[0].AttackDistanceWithOffset;
            _layerMask = LayerMask.GetMask("structures", "enemies");

            _manager = manager;

            _pataponsHitPointDisplay = Display.PataponsHitPointDisplay.Add(this);
        }
        public bool CanGoForward()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.right, PataponEnvironment.PataponSight, _layerMask);
            return (hit.collider == null || hit.distance > _marchiDistance) && Hazoron.Hazoron.GetClosestHazoronPosition() > transform.position.x;
        }

        internal void RemovePon(Patapon patapon)
        {
            if (_patapons.Remove(patapon))
            {
                _manager.RemovePon(patapon);
                _pataponsHitPointDisplay.OnDead(patapon, _patapons);
            }
        }
        public void UpdateHitPoint(Patapon patapon)
        {
            _pataponsHitPointDisplay.UpdateHitPoint(patapon);
        }
    }
}
