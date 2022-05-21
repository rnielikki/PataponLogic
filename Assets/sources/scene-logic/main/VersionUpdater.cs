using TMPro;
using UnityEngine;

namespace Assets.sources.scene_logic.main
{
    public class VersionUpdater : MonoBehaviour
    {
        void Start()
        {
            var textComponent = GetComponent<TextMeshProUGUI>();
            textComponent.text = textComponent.text.Replace("%Version%", Application.version);
        }
    }
}