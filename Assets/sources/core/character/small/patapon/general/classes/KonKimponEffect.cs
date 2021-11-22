namespace PataRoad.Core.Character.Patapons.General
{
    class KonKimponEffect : IGeneralEffect
    {
        private KonKimponGroupStatOperation _operation = new KonKimponGroupStatOperation();
        public void StartSelfEffect(Patapon patapon)
        {
            //See Robopon OnAttackHit.
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Add(_operation);
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Remove(_operation);
        }
        private class KonKimponGroupStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.CriticalResistance *= 2;
                input.DefenceMin *= 1.2f;
                input.DefenceMax *= 1.5f;
                return input;
            }
        }
    }
}
