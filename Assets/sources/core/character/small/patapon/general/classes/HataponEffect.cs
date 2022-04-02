namespace PataRoad.Core.Character.Patapons.General
{
    class HataponEffect : IGeneralEffect
    {
        readonly HataponGroupStatOperation _operation = new HataponGroupStatOperation();
        public void StartSelfEffect(Patapon patapon)
        {
            //it's on prefab stat so forget it.
        }
        public void StartGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Add(_operation);
        }
        public void EndGroupEffect(System.Collections.Generic.IEnumerable<Patapon> patapons)
        {
            foreach (var patapon in patapons) patapon.StatOperator.Remove(_operation);
        }
        private sealed class HataponGroupStatOperation : IStatOperation
        {
            public Stat Calculate(Rhythm.Command.CommandSong song, bool charged, Stat input)
            {
                input.AddStaggerResistance(GetValueToAdd(input.StaggerResistance));
                input.AddKnockbackResistance(GetValueToAdd(input.KnockbackResistance));
                input.AddFireResistance(GetValueToAdd(input.FireResistance));
                input.AddIceResistance(GetValueToAdd(input.IceResistance));
                input.AddSleepResistance(GetValueToAdd(input.SleepResistance));
                return input;
            }
            private float GetValueToAdd(float rate) => UnityEngine.Mathf.Abs(rate * 0.5f);
        }
    }
}
