using PataRoad.Core.Character;
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
            //Common.SceneLoadingAction.Create("Battle", true, "Submit", StartMission);
            //Common.SceneLoadingAction.Create("Patapolis", false, "Cancel", GoBack);

            foreach (var patapon in GetComponentsInChildren<PataponData>())
            {
                patapon.Animator.Play("walk");
                if (patapon.Type == Core.Character.Class.ClassType.Toripon)
                {
                    patapon.Animator.Play("tori-fly-stop");
                }
            }
            GetComponent<CharacterGroupNavigator>().Init();
        }
        private void StartMission()
        {
            Exit(_onEnter);
        }
        private void GoBack()
        {
            Exit(_onCancel);
        }
        private void Exit(AudioClip sound)
        {
            //And save data to static!
            Core.GlobalData.GlobalAudioSource.PlayOneShot(sound);
        }
    }
}
