
namespace Core.Character.Patapon
{
    /// <summary>
    /// All Patapon-related environment, such as marching distance.
    /// </summary>
    internal static class PataponEnvironment
    {
        //Marching info
        public const float PataponSight = UnityEngine.Mathf.Infinity; //After certain distance, Patapon can't find where is the enemy!
        public const float WalkingSteps = 4; //defines walking steps for one PATAPATA song.
        public const float Steps = WalkingSteps / Rhythm.RhythmEnvironment.TurnSeconds;
        private readonly static System.Collections.Generic.Dictionary<ClassType, float> _marchDistance =
            new System.Collections.Generic.Dictionary<ClassType, float>()
            {
                //--melee
                { ClassType.Tatepon, 0.1f},
                { ClassType.Dekapon, 0.2f},
                { ClassType.Robopon, 0.3f},
                { ClassType.Kibapon, 0.5f},
                //--range (short)
                { ClassType.Yaripon, 1},
                { ClassType.Megapon, 1.5f},
                { ClassType.Toripon, 2},
                //--range (long)
                { ClassType.Yumipon, 3},
                { ClassType.Mahopon, 3.5f},
            };
        public static float GetMarchDistance(ClassType classType) => _marchDistance[classType];
    }
}
