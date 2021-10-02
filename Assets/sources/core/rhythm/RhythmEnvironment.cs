namespace Core.Rhythm
{
    internal static class RhythmEnvironment
    {
        //Paths
        internal const string ThemePath = "Rhythm/themes/";
        internal const string DrumSoundPath = "Rhythm/sounds/drums/";

        //Rnages and intervals for one drum
        internal const float UpdateSecond = 0.01f; //"Fixed update" second
        internal const float InputInterval = 0.5f; //Can be changed for 1/8 beat minigame
        internal const float HalfInputInterval = InputInterval / 2;
        internal const float PerfectRange = 0.2f; //Was 0.05f
        internal const float GoodRange = 0.21f; //Was 0.1f
        internal const float BadRange = 0.25f;

        //Command
        internal const float Command = 4;
        internal const float TurnSeconds = Command * InputInterval;

        //Fever
        internal const int FeverPerfectRequirement = 4;
        internal const int FeverMaximum = 10;
    }
}
