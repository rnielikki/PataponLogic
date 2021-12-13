namespace PataRoad.Core.Character.Patapons.General
{
    class HataponEffect : IGeneralEffect
    {
        readonly HataponGroupStatOperation _operation = new HataponGroupStatOperation();
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
        private class HataponGroupStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.FireResistance = HalfOrDouble(input.FireResistance);
                input.IceResistance = HalfOrDouble(input.IceResistance);
                input.SleepResistance = HalfOrDouble(input.SleepResistance);
                return input;
            }
            private float HalfOrDouble(float rate) => rate < 0 ? rate / 1.5f : rate * 1.5f;
        }
    }
}
