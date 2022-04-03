using UnityEngine;

namespace Assets.sources.common.display
{
    public class CameraScreenSpaceInitializer : MonoBehaviour
    {
        [SerializeField]
        string _sortingLayerName;

        // Use this for initialization
        void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = _sortingLayerName;
        }
    }
}