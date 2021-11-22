
namespace PataRoad.Core.Character
{
    /// <summary>
    /// All Patapon-related environment, such as marching distance.
    /// </summary>
    internal static class CharacterEnvironment
    {
        //Distance info

        //------- DO NOT SET SIGHT TO SHORTER THAN LONGEST RANGE (Mahopon) DISTANCE!
        public const float Sight = 40; //After certain distance, Patapon can't find where is the enemy!

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
