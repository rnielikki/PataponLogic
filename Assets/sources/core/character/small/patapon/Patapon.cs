using PataRoad.Core.Character.Equipment.Weapon;
using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Represents any one Patapon. Classes will inherited from this class.
    /// </summary>
    public abstract class Patapon : SmallCharacter
    {
        /// <summary>
        /// Rarepon type of this Patapon.
        /// </summary>
        public Rarepon Rarepon { get; protected set; }

        /// <summary>
        /// Represents if PONCHAKA song is used before command (and in a row). This can be used for PONCHAKA~PONPON or PONCHAKA~CHAKACHAKA command.
        /// </summary>
        protected bool _charged { get; private set; }

        /// <summary>
        /// Current Patapon Index, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Current Patapon Index IN <see cref="PataponGroup"/>, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int IndexInGroup { get; internal set; }

        protected CommandSong _lastSong;
        private float _lastPerfectionPercent;

        public float AttackDistanceWithOffset => AttackDistance + CharacterSize;
        public bool IsGeneral { get; private set; }

        internal PataponRendererInfo RendererInfo { get; private set; }
        /// <summary>
        /// Sets Patapon distance.
        /// </summary>
        public PataponDistanceManager DistanceManager { get; private set; }
        protected PataponGroup _group { get; private set; }

        protected override void BeforeDie()
        {
            _group.RemovePon(this);
        }
        protected override void AfterDie()
        {
            _group.RemoveIfEmpty();
        }
        /// <summary>
        /// Remember call this on Awake() in inherited class
        /// </summary>
        protected override void Init()
        {
            base.Init();
            _group = GetComponentInParent<PataponGroup>();
            DistanceManager = GetComponent<PataponDistanceManager>();
            DistanceCalculator = DistanceManager.DistanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(this);
            CharAnimator = new CharacterAnimator(GetComponent<Animator>());
            InitDistanceFromHead();

            RendererInfo = new PataponRendererInfo(this, _bodyName);

            var general = GetComponent<PataponGeneral>();
            if (general != null) IsGeneral = true;
        }

        /// <summary>
        /// Sets distance from calculated Patapon head. Don't use this if the Patapon uses any vehicle.
        /// </summary>
        /// <param name="attackDistance">Attack distance, without considering head size.</param>
        protected void InitDistanceFromHead()
        {
            CharacterSize = transform.Find(_bodyName + "/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
        }

        public void MoveOnDrum(string drumName)
        {
            StopAttacking();
            CharAnimator.Animate(drumName);
            DistanceManager.StopMoving();
        }

        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="model">The command model, expected to be recieved from <see cref="PataponsManager"/>.</param>
        public virtual void Act(RhythmCommandModel model)
        {
            var song = model.Song;
            var isFever = model.ComboType == ComboStatus.Fever;
            if (_lastSong != song)
            {
                CharAnimator.ClearLateAnimation();
                AttackMoveData.WasHitLastTime = false;
            }
            _lastSong = song;
            _lastPerfectionPercent = model.Percentage;
            switch (song)
            {
                case CommandSong.Patapata:
                    Walk();
                    break;
                case CommandSong.Ponpon:
                    Attack(isFever);
                    break;
                case CommandSong.Chakachaka:
                    Defend(isFever);
                    break;
                case CommandSong.Ponpata:
                    Dodge();
                    break;
                case CommandSong.Ponchaka:
                    Charge();
                    break;
                case CommandSong.Dondon:
                    Jump();
                    break;
                case CommandSong.Donchaka:
                    Party();
                    break;
                case CommandSong.Patachaka:
                    CharAnimator.Animate("walk");
                    DistanceManager.MoveToInitialPlace(Stat.MovementSpeed * 2);
                    break;
            }
            _charged = song == CommandSong.Ponchaka; //Removes charged status if it isn't charging command
        }
        public void DoMisisonCompleteGesture()
        {
            StartCoroutine(PartyOnComplete());
            System.Collections.IEnumerator PartyOnComplete()
            {
                Walk();
                yield return new WaitForSeconds(0.25f * IndexInGroup);
                for (int i = 0; i < 3; i++)
                {
                    CharAnimator.AnimateFrom("party");
                    yield return new WaitForSeconds(4);
                    CharAnimator.AnimateFrom("walk");
                    yield return new WaitForSeconds(2);
                }
            }
        }

        /// <summary>
        /// Going back to Idle status. This also means ALL FEVER/COMMAND is CANCELED. also removes PONCHAKA <see cref="_charged"/> status.
        /// </summary>
        public virtual void PlayIdle()
        {
            _charged = false;
            StopAttacking();
            CharAnimator.Animate("Idle");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PATAPATA Input
        /// </summary>
        void Walk()
        {
            CharAnimator.Animate("walk");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PONPON Input
        /// </summary>
        protected virtual void Attack(bool isFever)
        {
            StartAttack("attack");
        }
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected virtual void Defend(bool isFever)
        {
            StartAttack("defend");
        }
        /// <summary>
        /// PONPATA Input
        /// </summary>
        protected virtual void Dodge()
        {
            CharAnimator.Animate("dodge");
            DistanceManager.MoveBack(Stat.MovementSpeed * 1.5f);
        }
        /// <summary>
        /// PONCHAKA Input
        /// </summary>
        protected virtual void Charge()
        {
            CharAnimator.Animate("charge");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }

        /// <summary>
        /// DONDON Input
        /// </summary>
        protected virtual void Jump()
        {
            CharAnimator.Animate("jump");
            DistanceManager.StopMoving();
        }

        /// <summary>
        /// DONCHAKA Input
        /// </summary>
        protected virtual void Party()
        {
            CharAnimator.Animate("party");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }

        protected override AttackMoveController SetAttackMoveController()
        {
            AttackMoveData = new PataponAttackMoveData(this);
            return base.SetAttackMoveController();
        }

        public override int GetAttackDamage()
        {
            return Mathf.RoundToInt(Mathf.Lerp(Stat.DamageMin, Stat.DamageMax, _lastPerfectionPercent));
        }
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _group.UpdateHitPoint(this);
        }
    }
}
