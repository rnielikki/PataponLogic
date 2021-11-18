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
                        input.Defence *= 6;
                    }
                    else
                    {
                        input.Defence *= 3;
                    }
                    input.DamageMin /= 5;
                    input.DamageMax /= 4;
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
