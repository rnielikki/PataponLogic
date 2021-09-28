namespace Core.Rhythm.Bgm
{
    /*
     * --------------------------------------------------Each theme folder must include:
     * intro.mp3 : first play when the game is loaded
     * base.mp3 : no command input
     * command.mp3 : command input, but not yet chance to enter fever, including 'Patapons saying' turn
     * before-fever-intro.mp3 : command input, first chance to enter fever. 2 sec
     * before-fever.mp3 : command input, chance to enter fever
     * fever-intro.mp3 : before shouting "Fever!"
     * fever.mp3 : music when on fever status
     * --------------------------------------------------Check the example from pipirichi folder!

     * NOTE: BE PRECISE TO SOURCE, KEEP ALL THE MUSIC TO %2==0 SECONDS!
    */
    /// <summary>
    /// Used by <see cref="RhythmBgmPlayer"/>, for indexing
    /// </summary>
    enum RhythmBgmIndex
    {
        Intro,
        Base,
        Command,
        BeforeFeverIntro,
        BeforeFever,
        FeverIntro,
        Fever
    }
}
