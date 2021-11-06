using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Provides data for <see cref="Display.PataponStatusRenderer"/>. <see cref="Patapon"/> generates and holds it.
    /// </summary>
    internal class PataponRendererInfo
    {
        private readonly GameObject[] _rendererObjects;
        public Transform BodyTransform { get; }
        public Vector2 BoundingOffset { get; private set; }
        public Vector2 Center => (Vector2)BodyTransform.position + BoundingOffset;

        private readonly GameObject[] _nonWeaponRendererObjects;
        private readonly GameObject[] _weaponRendererObjects;

        private readonly int _nonWeaponRendererLayerId;
        private readonly int _weaponRendererLayerId;

        internal PataponRendererInfo(Patapon patapon, string rootName)
        {
            var renderers = patapon.GetComponentsInChildren<SpriteRenderer>();
            var rendererObjects = renderers.Select(r => r.gameObject);
            _rendererObjects = rendererObjects.ToArray();
            int layer = patapon.gameObject.layer;

            List<GameObject> renderersNonWeapon = rendererObjects.ToList();
            List<GameObject> renderersWeapon = new List<GameObject>();
            foreach (var rendererObj in _rendererObjects)
            {
                if (rendererObj.layer != layer)
                {
                    renderersNonWeapon.Remove(rendererObj);
                    renderersWeapon.Add(rendererObj);
                }
            }
            _nonWeaponRendererObjects = renderersNonWeapon.ToArray();
            _weaponRendererObjects = renderersWeapon.ToArray();

            _nonWeaponRendererLayerId = layer;
            if (_weaponRendererObjects.Length != 0) _weaponRendererLayerId = _weaponRendererObjects[0].layer;

            BodyTransform = patapon.transform.Find(rootName);
            SetBoundingBoxSize(renderers);
        }

        private void SetBoundingBoxSize(Renderer[] renderers)
        {
            Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);
            foreach (var renderer in renderers)
            {
                min = Vector2.Min(min, renderer.bounds.min);
                max = Vector2.Max(max, renderer.bounds.max);
            }
            BoundingOffset = new Vector2((min.x + max.x) / 2, (min.y + max.y) / 2) - (Vector2)BodyTransform.position;
        }
        public void StartRenderMode(int layer)
        {
            foreach (var renderer in _rendererObjects)
            {
                renderer.layer = layer;
            }
        }
        public void EndRenderMode()
        {
            foreach (var rendererObj in _nonWeaponRendererObjects) rendererObj.layer = _nonWeaponRendererLayerId;
            foreach (var rendererObj in _weaponRendererObjects) rendererObj.layer = _weaponRendererLayerId;
        }
    }
}
