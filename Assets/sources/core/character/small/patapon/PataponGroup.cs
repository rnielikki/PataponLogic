using UnityEngine;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : UnityEngine.MonoBehaviour
    {
        public PataponGeneral General { get; private set; }
        public Patapon[] Patapons { get; private set; }
        public int Index { get; internal set; }

        public ClassType ClassType { get; internal set; }
        private float _marchiDistance;
        private LayerMask _layerMask;

        internal void Init()
        {
            if (Patapons != null) return;
            General = GetComponentInChildren<PataponGeneral>();
            Patapons = GetComponentsInChildren<Patapon>();
            _marchiDistance = Patapons[0].AttackDistanceWithOffset;
            _layerMask = LayerMask.GetMask("structures", "enemies");
        }
        public bool CanGoForward()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.right, PataponEnvironment.PataponSight, _layerMask);
            return (hit.collider == null || hit.distance > _marchiDistance) && Hazoron.Hazoron.GetClosestHazoronPosition() > transform.position.x;
        }
    }
}
