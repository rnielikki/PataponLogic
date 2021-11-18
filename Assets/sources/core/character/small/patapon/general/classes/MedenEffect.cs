namespace PataRoad.Core.Character.Patapons.General
{
    class MedenEffect : IGeneralEffect
    {
        readonly MedenStatOperation _operation = new MedenStatOperation();
        public void StartSelfEffect(Patapon patapon)
        {
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons)
            {
                patapon.StatOperator.Add(_operation);
            }
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons)
            {
                patapon.StatOperator.Remove(_operation);
            }
        }
        private class MedenStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                switch (song)
                {
                    case Rhythm.Command.CommandSong.Ponpon:
                        input.MultipleDamage(charged ? 1.5f : 1.2f);
                        break;
                    case Rhythm.Command.CommandSong.Chakachaka:
                        input.Defence *= charged ? 2.5f : 2;
                        break;
                }
                return input;
            }
        }
    }
}
