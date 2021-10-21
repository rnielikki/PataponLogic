using System.Collections.Generic;

namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : UnityEngine.MonoBehaviour
    {
        public PataponGeneral General { get; private set; }
        public Patapon[] Patapons { get; private set; }
        //--- this should be general but temp value for position and patapata test
        private Patapon _headPon;
        private void Awake()
        {
            General = GetComponentInChildren<PataponGeneral>();
            Patapons = GetComponentsInChildren<Patapon>();
            _headPon = Patapons[0];
        }
    }
}
