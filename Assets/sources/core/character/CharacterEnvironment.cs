
namespace PataRoad.Core.Character
{
    /// <summary>
    /// All Patapon-related environment, such as marching distance.
    /// </summary>
    internal static class CharacterEnvironment
    {
        //Distance info

        //------- DO NOT SET SIGHT TO SHORTER THAN LONGEST RANGE (Yumipon + wind) DISTANCE!
        public const float OriginalSight = 30; //After certain distance, Patapon can't find where is the enemy!
        public static float Sight { get; set; } = OriginalSight; //After certain distance, Patapon can't find where is the enemy!
        /// <summary>
        /// Attack distance multiplier of the animal. Affected by weather, especially RAIN.
        /// </summary>
        public static float AnimalSightMultiplier { get; set; } = 1; //This reacts to rain.

        public const float MaxAttackDistance = 20;
        /// <summary>
        /// Like Tatepon Ponchaka~Ponpon. This position is relative to the root Patapon position manager.
        /// </summary>
        public const int RushAttackDistance = 15;
        public const int DodgeDistance = RushAttackDistance;

        /// <summary>
        /// Max y height to scan, 0 to this value. About (a bit heigher than) Toripon height expected.
        /// </summary>
        public const float MaxYToScan = 6;
    }
}
