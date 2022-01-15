namespace PataRoad.Core.Rhythm
{
    public static class RhythmEnvironment
    {
        //Paths
        public const string ThemePath = "Rhythm/themes/";
        public const string DrumSoundPath = "Rhythm/sounds/drums/";

        //Rnages and intervals for one drum
        public const float InputInterval = 0.5f; //Can be changed for 1/8 beat minigame
        public const float HalfInputInterval = InputInterval / 2;

        public static float PerfectRange { get; private set; } = 0.23f; //Was 0.05f
        public static float GoodRange { get; private set; } = 0.24f; //Was 0.1f
        public static float BadRange { get; private set; } = 0.25f;
        public static float MiracleRange { get; private set; } = 0.12f;
        public static float MinEffectThreshold { get; private set; } = 0.2f;

        //Command
        public const float TurnSeconds = 4 * InputInterval;

        //Fever
        public const int FeverPerfectRequirement = 4;
        public static int FeverMaximum { get; private set; } = 10;
        public static Difficulty Difficulty { get; private set; }

        public static void ChangeDifficulty(Difficulty difficulty)
        {
            Difficulty = difficulty;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    PerfectRange = 0.15f;
                    GoodRange = 0.15f;
                    BadRange = 0.25f;
                    MiracleRange = 0.125f;
                    MinEffectThreshold = 0.17f;
                    FeverMaximum = 8;
                    break;
                case Difficulty.Normal:
                    PerfectRange = 0.02f;
                    GoodRange = 0.075f;
                    BadRange = 0.25f;
                    MiracleRange = 0.12f;
                    MinEffectThreshold = 0.1f;
                    FeverMaximum = 10;
                    break;
                case Difficulty.Hard:
                    PerfectRange = 0.01f;
                    GoodRange = 0.04f;
                    BadRange = 0.25f;
                    MiracleRange = 0.1f;
                    MinEffectThreshold = GoodRange;
                    FeverMaximum = 15;
                    break;
            }
        }
    }
}
