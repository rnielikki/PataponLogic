using System;
using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : MonoBehaviour
    {
        public PataponGeneral General { get; private set; }
        public System.Collections.Generic.List<Patapon> Patapons { get; private set; }
        public int Index { get; internal set; }

        public ClassType ClassType { get; internal set; }
        private float _marchiDistance;
        private LayerMask _layerMask;

        private PataponsManager _manager;

        internal void Init()
        {
            if (Patapons != null) return;
            General = GetComponentInChildren<PataponGeneral>();
            Patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            _marchiDistance = Patapons[0].AttackDistanceWithOffset;
            _layerMask = LayerMask.GetMask("structures", "enemies");

            _manager = GetComponentInParent<PataponsManager>();
        }
        public bool CanGoForward()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.right, PataponEnvironment.PataponSight, _layerMask);
            return (hit.collider == null || hit.distance > _marchiDistance) && Hazoron.Hazoron.GetClosestHazoronPosition() > transform.position.x;
        }

        internal void RemovePon(Patapon patapon)
        {
            if (Patapons.Remove(patapon))
            {
                _manager.RemovePon(patapon);
            }
        }
    }
}
