using UnityEngine;

namespace PataRoad.Core.Character.Bosses
{
    public class ShooshookleArmPart : MonoBehaviour, IBossPart
    {
        private ShooshookleArm _parent;
        private void Start()
        {
            _parent = GetComponentInParent<ShooshookleArm>();
        }
        public float TakeDamage(int damage)
        {
            _parent.TakeDamage(damage);
            return 0.8f;
        }
    }
}