namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : UnityEngine.MonoBehaviour
    {
        public PataponGeneral General { get; private set; }
        public Patapon[] Patapons { get; private set; }
        private DistanceCalculator _distanceCalculator;
        public int Index { get; internal set; }

        public ClassType ClassType { get; internal set; }
        private float _marchiDistance;

        internal void Init()
        {
            if (_distanceCalculator != null) return;
            _distanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(gameObject, 0);
            General = GetComponentInChildren<PataponGeneral>();
            Patapons = GetComponentsInChildren<Patapon>();
            _marchiDistance = Patapons[0].AttackDistanceWithOffset;
        }
        public bool CanGoForward()
        {
            var hit = _distanceCalculator.GetClosest();
            return (hit.collider == null || hit.distance > _marchiDistance) && Hazoron.Hazoron.GetClosestHazoronPosition() > transform.position.x;
        }
    }
}
