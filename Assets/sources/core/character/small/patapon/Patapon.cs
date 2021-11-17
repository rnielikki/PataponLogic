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
        /// Represents if PONCHAKA song is used before command (and in a row). This can be used for PONCHAKA~PONPON or PONCHAKA~CHAKACHAKA command.
        /// </summary>
        public bool Charged { get; private set; }

        /// <summary>
        /// Current Patapon Index, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Current Patapon Index IN <see cref="PataponGroup"/>, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int IndexInGroup { get; internal set; }

        public CommandSong LastSong { get; protected set; }
        private float _lastPerfectionPercent;

        public float AttackDistanceWithOffset => AttackDistance + CharacterSize;
        public bool IsGeneral { get; private set; }

        internal PataponRendererInfo RendererInfo { get; private set; }
        /// <summary>
        /// Sets Patapon distance.
        /// </summary>
        public PataponDistanceManager DistanceManager { get; private set; }
        protected PataponGroup _group { get; private set; }
        public override float AttackDistance => Weapon.MinAttackDistance + Weapon.WindAttackDistanceOffset * (Map.Weather.WeatherInfo.Wind?.AttackOffsetOnWind ?? 0.5f);
        public override Vector2 MovingDirection => Vector2.right;

        public bool OnFever { get; private set; }
        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.PataponSounds;


        /// <summary>
        /// Stat before going through pipeline.
        /// </summary>
        protected Stat _realStat;
        public StatOperator StatOperator { get; private set; }

        public abstract General.IGeneralEffect GetGeneralEffect();

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
            //--- initialise sats.
            _realStat = _defaultStat;
            StatOperator = new StatOperator(_realStat);
            StatOperator.Add(new PataponStatOperation(this));

            //--- init
            base.Init();
            _group = GetComponentInParent<PataponGroup>();
            DistanceManager = GetComponent<PataponDistanceManager>();
            DistanceCalculator = DistanceManager.DistanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(this);
            InitDistanceFromHead();
            StatusEffectManager.SetRecoverAction(() =>
            {
                if (!TurnCounter.IsPlayerTurn) PerformCommandAction(LastSong);
                else if (!TurnCounter.IsOn) DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
            });

            RendererInfo = new PataponRendererInfo(this, BodyName);

            var general = GetComponent<General.PataponGeneral>();
            if (general != null) IsGeneral = true;
        }

        /// <summary>
        /// Sets distance from calculated Patapon head. Don't use this if the Patapon uses any vehicle.
        /// </summary>
        protected void InitDistanceFromHead()
        {
            CharacterSize = transform.Find(BodyName + "/Face").GetComponent<CircleCollider2D>().radius + 0.1f;
        }

        public void MoveOnDrum(string drumName)
        {
            StopAttacking();
            CharAnimator.Animate(drumName);
        }

        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="model">The command model, expected to be recieved from <see cref="PataponsManager"/>.</param>
        public virtual void Act(RhythmCommandModel model)
        {
            var song = model.Song;
            OnFever = model.ComboType == ComboStatus.Fever;
            if (LastSong != song)
            {
                CharAnimator.ClearLateAnimation();
                AttackMoveData.WasHitLastTime = false;
            }
            LastSong = song;
            _lastPerfectionPercent = model.Percentage;

            Stat = StatOperator.GetFinalStat();
            if (!StatusEffectManager.OnStatusEffect || LastSong == CommandSong.Donchaka)
            {
                PerformCommandAction(song);
            }
            //Should be executed AFTER command execusion, for avoiding command bug
            Charged = song == CommandSong.Ponchaka; //Removes charged status if it isn't charging command
            StatusEffectManager.IgnoreStatusEffect = song == CommandSong.Donchaka;
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
        /// Going back to Idle status. This also means ALL FEVER/COMMAND is CANCELED. also removes PONCHAKA <see cref="Charged"/> status.
        /// </summary>
        public virtual void PlayIdle()
        {
            Stat = _realStat;
            OnFever = false;
            Charged = false;
            StatusEffectManager.IgnoreStatusEffect = false;
            StopAttacking();
            CharAnimator.Animate("Idle");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public override void StopAttacking()
        {
            base.StopAttacking();
            DistanceManager.StopMoving();
        }
        private void PerformCommandAction(CommandSong song)
        {
            switch (song)
            {
                case CommandSong.Patapata:
                    Walk();
                    break;
                case CommandSong.Ponpon:
                    Attack();
                    break;
                case CommandSong.Chakachaka:
                    Defend();
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
        protected virtual void Attack()
        {
            StartAttack("attack");
        }
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected virtual void Defend()
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
        /// Some range attack (like Mahopon or Yumipon) will use it. This prevents unnecessary moving while use many PONCHAKA~PONPON.
        /// </summary>
        /// <note>NOT all range unit use this.</note>
        protected void ChargeWithoutMoving()
        {
            CharAnimator.Animate("charge");
            DistanceManager.StopMoving();
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
            StatusEffectManager.Recover();
            CharAnimator.Animate("party");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }

        protected override AttackMoveController SetAttackMoveController()
        {
            AttackMoveData = new PataponAttackMoveData(this);
            return base.SetAttackMoveController();
        }

        public override int GetAttackDamage(Stat stat)
        {
            return Mathf.RoundToInt(Mathf.Lerp(stat.DamageMin, stat.DamageMax, _lastPerfectionPercent));
        }
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _group.UpdateHitPoint(this);
        }
        /// <summary>
        /// DON'T CALL THIS METHOD SEPARATELY. CALL ONLY IN <see cref="PataponGroup"/>. Otherwise heal status won't displayed!
        /// </summary>
        internal void Heal(PataponGroup sender, int amount)
        {
            if (sender != _group) return;
            CurrentHitPoint = Mathf.Clamp(amount, CurrentHitPoint + amount, Stat.HitPoint);
        }
        //------------------
        protected void WeaponLoadTest(string path, int index)
        {
            if (!IsGeneral)
            {
                var data = Items.ItemLoader.GetItem(Items.ItemType.Equipment, path, index) as Items.EquipmentData;
                if (data != null) EquipmentManager.Equip(data, _realStat);
                else Debug.Log("data is null :(");
            }
        }
    }
}
