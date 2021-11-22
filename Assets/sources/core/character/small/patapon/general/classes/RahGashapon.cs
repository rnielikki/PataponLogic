namespace PataRoad.Core.Character.Patapons.General
{
    class RahGashaponEffect : IGeneralEffect
    {
        public void StartSelfEffect(Patapon patapon)
        {
            patapon.StatOperator.Add(new RahGashaSelfStatOperation());
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            //See Tatepon.OnAttackHit instead
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            //See Tatepon.OnAttackHit instead
        }
        private class RahGashaSelfStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                switch (song)
                {
                    case Rhythm.Command.CommandSong.Ponpon:
                        if (!charged) input.AttackSeconds /= 2;
                        break;
                    case Rhythm.Command.CommandSong.Chakachaka:
                        input.DefenceMin *= 2;
                        input.DefenceMax *= 2;
                        break;
                }
                return input;
            }
        }

    }
}
