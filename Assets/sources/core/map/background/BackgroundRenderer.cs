using UnityEngine;
using UnityEngine.UI;

public class BackgroundRenderer : MonoBehaviour
{
    Image _image;
    private Transform _cameraTransform;
    [SerializeField]
    [Tooltip("The greater the value is, it moves fast when camera move")]
    private float _movingOffset;
    void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _image = GetComponent<Image>();
        //Still resize problem remains. If you can handle shader language, please replace to shader...
        var mainTex = _image.material.mainTexture;
        var rect = GetComponent<RectTransform>().rect;
        var parentRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
        _image.material.SetTextureScale("_MainTex", new Vector2(mainTex.height * rect.width / (mainTex.width * rect.height), 1));
    }

    // Update is called once per frame
    void Update()
    {
        _image.material.SetTextureOffset("_MainTex", Vector2.right * _cameraTransform.position.x * _movingOffset);
    }
}
