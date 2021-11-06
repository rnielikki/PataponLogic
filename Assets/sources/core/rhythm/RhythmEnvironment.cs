namespace PataRoad.Core.Rhythm
{
    public static class RhythmEnvironment
    {
        //Paths
        public const string ThemePath = "Rhythm/themes/";
        public const string DrumSoundPath = "Rhythm/sounds/drums/";

        //Rnages and intervals for one drum
        public const float UpdateSecond = 0.01f; //"Fixed update" second
        public const float InputInterval = 0.5f; //Can be changed for 1/8 beat minigame
        public const float HalfInputInterval = InputInterval / 2;
        public const float PerfectRange = 0.1f; //Was 0.05f
        public const float GoodRange = 0.2f; //Was 0.1f
        public const float BadRange = 0.25f;

        //Command
        public const float TurnSeconds = 4 * InputInterval;

        //Fever
        public const int FeverPerfectRequirement = 4;
        public const int FeverMaximum = 10;
    }
}
