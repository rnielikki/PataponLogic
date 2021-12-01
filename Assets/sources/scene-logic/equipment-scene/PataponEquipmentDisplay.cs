using UnityEngine;

namespace PataRoad.SceneLogic.EquipmentScene
{
    public class PataponEquipmentDisplay : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _onEnter;
        [SerializeField]
        private AudioClip _onCancel;
        private AudioSource _audioSource;
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            Core.Character.Patapons.PataponGroupGenerator.Generate(Core.GlobalData.PataponInfo.CurrentClasses, transform);
            Common.SceneLoadingAction.Create("Battle", true, "Submit", () => Core.GlobalData.GlobalAudioSource.PlayOneShot(_onEnter));
            Common.SceneLoadingAction.Create("Patapolis", false, "Cancel", () => Core.GlobalData.GlobalAudioSource.PlayOneShot(_onCancel));
        }
    }
}
