using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class LaserDropStaff : AttackCollisionStaff
    {
        private bool _updatingLaser;
        private LineRenderer _lineRenderer;
        private float _width;

        public override void Initialize(SmallCharacter holder)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            base.Initialize(holder);
        }
        public override void SetElementalColor(Color color)
        {
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.75f, 0), new GradientAlphaKey(1f, 1f) }
                );
            _lineRenderer.colorGradient = gradient;
            base.SetElementalColor(color);
        }
        protected override void PerformAttack()
        {
            _updatingLaser = true;
            SetImageColor(0);
            SetLineWidth(0);

            base.PerformAttack();
            var pos = transform.position;
            pos.y = -1;
            transform.position = pos;
        }
        private void SetLineWidth(float width)
        {
            _width = Mathf.Min(width, 1);
            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;
        }
        private void SetImageColor(float alpha)
        {
            var clr = _image.color;
            clr.a = alpha;
            _image.color = clr;
        }
        private void Update()
        {
            if (_updatingLaser)
            {
                SetLineWidth(_width + (Time.deltaTime * 1 / _attackSeconds) + 0.1f);
                SetImageColor(_width);
                if (_width == 1)
                {
                    _updatingLaser = false;
                }
            }
        }
    }
}
