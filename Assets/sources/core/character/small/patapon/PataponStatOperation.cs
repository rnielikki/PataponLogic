using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons
{
    internal class PataponStatOperation : IStatOperation
    {
        private readonly Patapon _patapon;
        internal PataponStatOperation(Patapon patapon) => _patapon = patapon;
        public Stat Calculate(Stat input)
        {
            switch (_patapon.LastSong)
            {
                case CommandSong.Ponpon:
                    if (_patapon.Charged)
                    {
                        input.MultipleDamage(3);
                    }
                    break;
                case CommandSong.Chakachaka:
                    if (_patapon.Charged)
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
            if (_patapon.Charged)
            {
                input.MovementSpeed *= 1.5f;
            }
            return input;
        }
    }
}
