using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    class Horn : WeaponObject
    {
        private ParticleSystem _attackParticles;
        private ParticleSystem _feverAttackParticles;
        private void Awake()
        {
            Init();
            _attackParticles = transform.Find("Attack").GetComponent<ParticleSystem>();
            _feverAttackParticles = transform.Find("FeverAttack").GetComponent<ParticleSystem>();
        }

        public override void Attack(AttackType attackType)
        {
            var main = _attackParticles.main;
            int startSpeed = 0;
            int emitCount = 0;
            switch (attackType)
            {
                case AttackType.FeverAttack:
                    AttackFever();
                    return;
                case AttackType.Attack:
                    //Attack is called in two times in animation, so doesn't need so many emit count.
                    startSpeed = 6;
                    emitCount = 5;
                    break;
                case AttackType.Defend:
                    startSpeed = 3;
                    emitCount = 5;
                    break;
                case AttackType.Charge:
                    startSpeed = 2;
                    emitCount = 2;
                    break;
            }
            main.startSpeed = startSpeed;
            _attackParticles.Emit(emitCount);
        }
        private void AttackFever()
        {
            _feverAttackParticles.Play();
        }
    }
}
