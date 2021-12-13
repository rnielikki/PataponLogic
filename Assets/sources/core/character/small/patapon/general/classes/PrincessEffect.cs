namespace PataRoad.Core.Character.Patapons.General
{
    class PrincessEffect : IGeneralEffect
    {
        private readonly PrincessGroupStatOperation _operation = new PrincessGroupStatOperation();
        public void StartSelfEffect(Patapon patapon)
        {
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Add(_operation);
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Remove(_operation);
        }
        private class PrincessGroupStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.Critical += 0.5f;
                return input;
            }
        }
    }
}
