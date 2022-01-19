using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons
{
    internal class PataponStatOperation : IStatOperation
    {
        private readonly Patapon _patapon;
        internal PataponStatOperation(Patapon patapon) => _patapon = patapon;
        public Stat Calculate(CommandSong song, bool charged, Stat input)
        {
            switch (song)
            {
                case CommandSong.Ponpon:
                    if (charged)
                    {
                        input.MultipleDamage(3);
                    }
                    break;
                case CommandSong.Chakachaka:
                    if (charged)
                    {
                        input.DefenceMin *= 6;
                        input.DefenceMax *= 6.5f;
                        input.BoostResistance(_patapon.LastPerfectionRate * 0.3f);
                    }
                    else
                    {
                        input.DefenceMin *= 2.7f;
                        input.DefenceMax *= 3;
                        input.BoostResistance(_patapon.LastPerfectionRate * 0.15f);
                    }
                    input.DamageMin /= 5;
                    input.DamageMax /= 4;
                    break;
                case CommandSong.Ponpata:
                    input.MovementSpeed *= (0.8f + _patapon.LastPerfectionRate);
                    break;
                default:
                    return input;
            }
            if (_patapon.OnFever)
            {
                input.MultipleDamage(1.5f);
            }
            if (charged)
            {
                input.MovementSpeed *= 1.5f;
            }
            return input;
        }
    }
}
