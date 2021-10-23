namespace Core.Character.Patapon
{
    /// <summary>
    /// Represents Patapon group.
    /// </summary>
    public class PataponGroup : UnityEngine.MonoBehaviour
    {
        public PataponGeneral General { get; private set; }
        public Patapon[] Patapons { get; private set; }
        private DistanceCalculator _distanceCalculator;
        public int Index { get; internal set; }

        //temporary serializefield until auto generated.
        [UnityEngine.SerializeField]
        private ClassType _classType;
        private float _marchiDistance;

        //--- this should be general but temp value for position and patapata test
        private void Awake()
        {
            _distanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(gameObject);
            _marchiDistance = PataponEnvironment.GetMarchDistance(_classType);
            General = GetComponentInChildren<PataponGeneral>();
            Patapons = GetComponentsInChildren<Patapon>();

            for (int i = 0; i < Patapons.Length; i++)
            {
                Patapons[i].GroupIndex = i;
            }
        }
        public bool CanGoForward() => _distanceCalculator.GetClosest().distance > _marchiDistance;
    }
}
