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
            var index = _patapons.IndexOf(patapon);
            if (index >= 0)
            {
                for (int i = index + 1; i < _patapons.Count; i++)
                {
                    _patapons[i].IndexInGroup--;
                    _patapons[i].DistanceManager.UpdateDefaultPosition();
                }
                _patapons.RemoveAt(index);
                _manager.RemovePon(patapon);
                _pataponsHitPointDisplay.OnDead(patapon, _patapons);
            }
            if (_patapons.Count == 0)
            {
                _manager.RemoveGroup(this);
            }
        }
        public void UpdateHitPoint(Patapon patapon)
        {
            _pataponsHitPointDisplay.UpdateHitPoint(patapon);
        }
        internal void MoveTo(int newIndex, bool smoothMove)
        {
            if (newIndex < 0) return;
            if (smoothMove)
            {
                StopAllCoroutines();
                StartCoroutine(MoveSmooth());
            }
            else
            {
                transform.localPosition = newIndex * PataponEnvironment.GroupDistance * Vector3.left;
            }
            System.Collections.IEnumerator MoveSmooth()
            {
                var targetX = newIndex * PataponEnvironment.GroupDistance;
                var target = targetX * Vector2.left;
                while (transform.localPosition.x != targetX)
                {
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, PataponEnvironment.Steps * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
