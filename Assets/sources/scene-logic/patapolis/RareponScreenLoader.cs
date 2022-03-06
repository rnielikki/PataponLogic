using PataRoad.SceneLogic.CommonSceneLogic;
using UnityEngine;

namespace PataRoad.SceneLogic.Patapolis
{
    public class RareponScreenLoader : MonoBehaviour
    {
        [SerializeField]
        RareponSelector _rareponSelector;
        public void Open()
        {
            StartCoroutine(Core.Global.GlobalData.GlobalInputActions.WaitForNextInput(() =>
            {
                _rareponSelector.Open(Core.Global.GlobalData.CurrentSlot.PataponInfo.RareponInfo.DefaultRarepon.Data);
            }));
        }
    }
}
