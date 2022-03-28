namespace PataRoad.Core.Character.Bosses
{
    public class GaruruSummon : SummonedBoss
    {
        private GaruruBall _ball;
        protected override void Chakachaka()
        {
            CharAnimator.Animate("ice");
            BoostDefence(0.5f);
        }

        protected override void ChargedChakachaka()
        {
            CharAnimator.Animate("ice");
            BoostDefence(1);
        }
        protected override void Ponpon()
        {
            _ball.Show();
            CharAnimator.Animate("ball");
            BoostAttack(10);
        }
        protected override void ChargedPonpon()
        {
            _ball.Show();
            CharAnimator.Animate("ball");
            BoostAttack(15);
        }
        private void BoostDefence(float amount)
        {
            foreach (var patapon in Patapons.PataponsManager.Current.Patapons)
            {
                if (patapon != null && !patapon.IsDead)
                {
                    patapon.Stat.DefenceMin += amount;
                    patapon.Stat.DefenceMax += amount;
                    patapon.Stat.BoostResistance(amount);
                }
            }
        }
        private void BoostAttack(float amount)
        {
            foreach (var patapon in Patapons.PataponsManager.Current.Patapons)
            {
                if (patapon != null && !patapon.IsDead)
                {
                    patapon.Stat.AddDamage(amount);
                }
            }
        }

        protected override void OnDead()
        {
            //hmm...
        }

        protected override void OnStarted()
        {
            _ball = GetComponentInChildren<GaruruBall>(true);
            var particles = _ball.GetComponent<UnityEngine.ParticleSystem>();
            var main = particles.main;
            main.loop = true;
            main.playOnAwake = true;
            main.prewarm = true;
            particles.Play();
            _ball.gameObject.SetActive(false);
            //nothing!
        }
    }
}