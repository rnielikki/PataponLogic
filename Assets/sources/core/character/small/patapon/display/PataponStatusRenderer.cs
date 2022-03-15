using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Display
{
    public class PataponStatusRenderer : MonoBehaviour
    {
        private int _renderingLayer;

        private PataponRendererInfo _rendererInfo;
        private float _zOffset;
        private Patapon _pon; //should check if pon is dead because of coroutine death and will be updated later

        private void Awake()
        {
            _renderingLayer = LayerMask.NameToLayer("patapon.rendering");
            _zOffset = transform.position.z;
        }

        void OnPreCull()
        {
            if (!_pon.IsReallyDead) _rendererInfo.StartRenderMode(_renderingLayer);
        }

        void OnPostRender()
        {
            if (!_pon.IsReallyDead) _rendererInfo.EndRenderMode();
        }
        private void Update()
        {
            if (_pon.IsReallyDead) return;
            Vector3 pos = _rendererInfo.Center;
            pos.z = _zOffset;
            transform.position = pos;
        }
        public void Init(Patapon patapon, RenderTexture renderTexture)
        {
            SetTarget(patapon);
            GetComponent<Camera>().targetTexture = renderTexture;
        }

        internal void SetTarget(Patapon patapon)
        {
            _pon = patapon;
            _rendererInfo = patapon.RendererInfo;
        }
    }
}
