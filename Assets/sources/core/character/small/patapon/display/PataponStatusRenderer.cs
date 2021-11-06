using UnityEngine;

namespace PataRoad.Core.Character.Patapons.Display
{
    public class PataponStatusRenderer : MonoBehaviour
    {
        private int _renderingLayer;

        private PataponRendererInfo _rendererInfo;
        private float _zOffset;

        private void Awake()
        {
            _renderingLayer = LayerMask.NameToLayer("patapon.rendering");
            _zOffset = transform.position.z;
        }

        void OnPreCull()
        {
            _rendererInfo.StartRenderMode(_renderingLayer);
        }

        void OnPostRender()
        {
            _rendererInfo.EndRenderMode();
        }
        private void Update()
        {
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
            _rendererInfo = patapon.RendererInfo;
        }
    }
}
