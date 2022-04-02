namespace PataRoad.Core.Character.Patapons.General
{
    class TonKamponEffect : IGeneralEffect
    {
        private readonly TonKamponGroupStatOperation _operation = new TonKamponGroupStatOperation();
        public void StartSelfEffect(Patapon patapon)
        {
            patapon.StatOperator.Add(new TonKamponSelfStatOperation());
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Add(_operation);
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Remove(_operation);
        }
        private sealed class TonKamponSelfStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.AddKnockbackResistance(UnityEngine.Mathf.Infinity);
                return input;
            }
        }
        private sealed class TonKamponGroupStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.MovementSpeed *= 1.5f;
                return input;
            }
        }
    }
}
