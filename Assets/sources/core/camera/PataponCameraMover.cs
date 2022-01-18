using UnityEngine;

namespace PataRoad.Core.CameraController
{
    class PataponCameraMover : CameraMover
    {
        internal Character.Patapons.PataponsManager Manager { get; set; }
        public override Transform Target => Manager.FirstPatapon?.transform ?? Manager.transform;
    }
}
