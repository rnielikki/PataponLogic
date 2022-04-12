﻿using PataRoad.Core.Rhythm.Command;

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
                        input.MultipleDamage(2.1f);
                        input.Critical += 0.25f + _patapon.LastPerfectionRate * 0.4f;
                    }
                    break;
                case CommandSong.Chakachaka:
                    if (charged)
                    {
                        input.DefenceMin *= 3.75f;
                        input.DefenceMax *= 4.25f;
                        input.BoostResistance(_patapon.LastPerfectionRate * 0.3f);
                    }
                    else
                    {
                        input.DefenceMin *= 1.64f;
                        input.DefenceMax *= 2.2f;
                        input.BoostResistance(_patapon.LastPerfectionRate * 0.15f);
                    }
                    input.DamageMin /= 5;
                    input.DamageMax /= 4;
                    break;
                case CommandSong.Ponpata:
                    input.MovementSpeed *= (PataponsManager.DodgeSpeedMinimumMultiplier + _patapon.LastPerfectionRate);
                    return input;
                default:
                    return input;
            }
            if (_patapon.OnFever)
            {
                input.MultipleDamage(1.5f);
            }
            if (charged)
            {
                input.MovementSpeed = 18;
                input.AttackSeconds = UnityEngine.Mathf.Min(2, input.AttackSeconds);
            }
            return input;
        }
    }
}
