
namespace Core.Character.Patapon
{
    /// <summary>
    /// All Patapon-related environment, such as marching distance.
    /// </summary>
    internal static class PataponEnvironment
    {
        //Marching info
        public const float PataponSight = 50; //After certain distance, Patapon can't find where is the enemy!
        public const float WalkingSteps = 6; //defines walking steps for one PATAPATA song.
        public const float Steps = WalkingSteps / Rhythm.RhythmEnvironment.TurnSeconds;

        //Distance info

        /// <summary>
        /// Distance between Patapons while attacking. This is used for Patapon distance in attacking or defending animation.
        /// </summary>
        public const float AttackDistanceBetweenPatapons = 0.5f;
        /// <summary>
        /// Distance between Patapons while idle. This is used for generating Patapons.
        /// </summary>
        public const float PataponIdleDistance = 1.5f;
        public const float GroupDistance = 6.5f;
    }
}
