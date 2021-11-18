using UnityEngine;

namespace PataRoad.Core.Character.Patapons.General
{
    public class GeneralCounterDisplay : MonoBehaviour
    {
        private PataponRendererInfo _renderer;
        private Vector3 _offset;
        private void Awake()
        {
            _renderer = GetComponentInParent<Patapon>().RendererInfo;
            _offset = new Vector3(0, _renderer.BoundingOffset.y, 0);
        }
        // Update is called once per frame
        void Update()
        {
            transform.position = (Vector3)_renderer.Center + _offset;
        }
    }
}
