using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : MonoBehaviour
    {
        public General.PataponGeneral General { get; private set; }

        private System.Collections.Generic.List<Patapon> _patapons;

        public System.Collections.Generic.IEnumerable<Patapon> Patapons => _patapons;

        private Display.PataponsHitPointDisplay _pataponsHitPointDisplay;
        public Class.ClassType ClassType { get; internal set; }
        private float _marchDistance;
        private LayerMask _layerMask;

        private PataponsManager _manager;

        internal void Init(PataponsManager manager)
        {
            if (_patapons != null) return;
            General = GetComponentInChildren<General.PataponGeneral>();
            _patapons = new System.Collections.Generic.List<Patapon>(GetComponentsInChildren<Patapon>());
            var sample = _patapons[0];
            _marchDistance = sample.AttackDistanceWithOffset;
            _layerMask = sample.DistanceCalculator.LayerMask;

            _manager = manager;

            _pataponsHitPointDisplay = Display.PataponsHitPointDisplay.Add(this);
        }
        public bool CanGoForward()
        {
            var closest = _patapons[0].DistanceCalculator.GetClosest();
            return closest == null || (closest.Value.x > transform.position.x + _marchDistance && Hazorons.Hazoron.GetClosestHazoronPosition() > transform.position.x);
        }

        internal void RemovePon(Patapon patapon)
        {
            var index = _patapons.IndexOf(patapon);
            if (index < 0) return;
            for (int i = index + 1; i < _patapons.Count; i++)
            {
                _patapons[i].IndexInGroup--;
                (_patapons[i].DistanceManager as PataponDistanceManager).UpdateDefaultPosition();
            }
            _pataponsHitPointDisplay.OnDead(patapon, _patapons);
            if (patapon.IsGeneral) General = null;
            _patapons.RemoveAt(index);
            _manager.RemovePon(patapon);
        }
        /// <summary>
        /// Removes the *Whole group* if it's empty.
        /// </summary>
        public void RemoveIfEmpty()
        {
            if (_patapons.Count == 0)
            {
                StopAllCoroutines();
                _manager.RemoveGroup(this);
            }
        }
        public void UpdateHitPoint(Patapon patapon)
        {
            _pataponsHitPointDisplay.UpdateHitPoint(patapon);
        }
        public void RefreshDisplay() => _pataponsHitPointDisplay.Refresh(_patapons);
        public void HealAllInGroup(int amount)
        {
            foreach (var patapon in _patapons)
            {
                patapon.Heal(this, amount);
            }
            _pataponsHitPointDisplay.Refresh(_patapons);
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
                transform.localPosition += PataponEnvironment.GroupDistance * Vector3.right;
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
