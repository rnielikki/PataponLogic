using PataRoad.Core.Character.Equipments.Weapons;
using PataRoad.Core.Rhythm.Command;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Represents any one Patapon. Classes will inherited from this class.
    /// </summary>
    public class Patapon : SmallCharacter
    {
        /// <summary>
        /// Current Patapon Index, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Current Patapon Index IN <see cref="PataponGroup"/>, from first of the line to the end of the line. Index starts from 0.
        /// </summary>
        public int IndexInGroup { get; internal set; }

        public CommandSong LastSong { get; protected set; }
        public float LastPerfectionRate { get; private set; }

        public float AttackDistanceWithOffset => AttackDistance + CharacterSize;
        public bool IsGeneral { get; private set; }

        internal PataponRendererInfo RendererInfo { get; private set; }
        public PataponGroup Group { get; private set; }
        public override float DefaultWorldPosition => DistanceManager.DefaultWorldPosition;
        public override float AttackDistance => Weapon.GetAttackDistance();
        public override Vector2 MovingDirection => Vector2.right;

        public bool Eaten { get; private set; }
        public override CharacterSoundsCollection Sounds => CharacterSoundLoader.Current.PataponSounds;

        /// <summary>
        /// Stat before going through pipeline.
        /// </summary>
        protected Stat _realStat;
        public StatOperator StatOperator { get; private set; }
        private int _attackTypeIndex;
        public override int AttackTypeIndex => _attackTypeIndex;

        public void BeEaten()
        {
            Eaten = true;
            Die();
        }

        public void BeTaken()
        {
            Eaten = true;
            MarkAsDead();
            Die();
            Group.RemoveIfEmpty();
        }

        protected override void BeforeDie()
        {
            Group.RemovePon(this);
        }
        protected override void AfterDie()
        {
            Group.RemoveIfEmpty();
            if (!Eaten) Items.DeadPataponItemDrop.Create(transform.position, IsGeneral);
        }
        private void Awake()
        {
            //--- init
            Init();
            _attackTypeIndex = Global.GlobalData.CurrentSlot.PataponInfo.GetAttackTypeIndex(_data.Type);
            IsGeneral = (_data as PataponData).IsGeneral;
            _realStat = _data.Stat;
            StatOperator = new StatOperator(_realStat);
            StatOperator.Add(new PataponStatOperation(this));
            Group = GetComponentInParent<PataponGroup>();
            DistanceCalculator = DistanceCalculator.GetPataponDistanceCalculator(this);
            DistanceManager = gameObject.AddComponent<PataponDistanceManager>();
            DistanceManager.DistanceCalculator = DistanceCalculator;

            StatusEffectManager.AddRecoverAction(() =>
            {
                if (Map.MissionPoint.IsMissionEnd && Map.MissionPoint.IsMissionSuccess) DoMissionCompleteGesture();
                else if (!TurnCounter.IsPlayerTurn) PerformCommandAction(LastSong);
                else if (!TurnCounter.IsOn) DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
            });
            RendererInfo = new PataponRendererInfo(this, BodyName);

            var general = GetComponent<General.PataponGeneral>();
            if (general != null) IsGeneral = true;

        }
        private void Start()
        {
            ClassData.InitLate(_realStat);
        }
        public UnityEngine.Events.UnityEvent CreateAfterDeathEvent()
        {
            _onAfterDeath = new UnityEngine.Events.UnityEvent();
            return _onAfterDeath;
        }

        public void MoveOnDrum(string drumName)
        {
            StopAttacking(true);
            if (LastSong != CommandSong.Ponpon && LastSong != CommandSong.Chakachaka
                && !Charged)
            {
                DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
            }
            CharAnimator.Animate(drumName);
        }

        /// <summary>
        /// Recieves command song and starts corresponding moving.
        /// </summary>
        /// <param name="model">The command model, expected to be recieved from <see cref="PataponsManager"/>.</param>
        public void Act(RhythmCommandModel model)
        {
            var song = model.Song;
            OnFever = model.ComboType == ComboStatus.Fever;
            if (LastSong != song)
            {
                CharAnimator.ClearLateAnimation();
                ClassData.AttackMoveData.WasHitLastTime = false;
            }
            LastSong = song;
            LastPerfectionRate = model.AccuracyRate;

            Stat = StatOperator.GetFinalStat(song, Charged);
            if (!StatusEffectManager.IsOnStatusEffect || LastSong == CommandSong.Donchaka)
            {
                PerformCommandAction(song);
            }
            //Should be executed AFTER command execusion, for avoiding command bug
            Charged = song == CommandSong.Ponchaka; //Removes charged status if it isn't charging command
            StatusEffectManager.IgnoreStatusEffect = song == CommandSong.Donchaka;
            ClassData.OnAction(model);
        }
        public void DoMissionCompleteGesture()
        {
            StopAttacking(false);
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
        public void PlayIdle()
        {
            Stat = _realStat;
            OnFever = false;
            Charged = false;
            StatusEffectManager.IgnoreStatusEffect = false;
            StopAttacking(false);
            if (!StatusEffectManager.IsOnStatusEffect) CharAnimator.Animate("Idle");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
            ClassData.OnCanceled();
        }
        public override void StopAttacking(bool pause)
        {
            base.StopAttacking(pause);
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
        protected void Walk()
        {
            CharAnimator.Animate("walk");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        /// <summary>
        /// PONPON Input
        /// </summary>
        protected void Attack() => ClassData.Attack();
        /// <summary>
        /// CHAKACHAKA Input
        /// </summary>
        protected void Defend() => ClassData.Defend();
        /// <summary>
        /// PONPATA Input
        /// </summary>
        protected void Dodge()
        {
            CharAnimator.Animate("dodge");
            DistanceManager.MoveBack(Stat.MovementSpeed * 1.5f);
        }
        /// <summary>
        /// PONCHAKA Input
        /// </summary>
        protected void Charge()
        {
            CharAnimator.Animate("charge");
        }

        /// <summary>
        /// DONDON Input
        /// </summary>
        protected void Jump()
        {
            CharAnimator.Animate("jump");
            DistanceManager.StopMoving();
        }

        /// <summary>
        /// DONCHAKA Input
        /// </summary>
        protected void Party()
        {
            StatusEffectManager.RecoverAndIgnoreEffect();
            CharAnimator.Animate("party");
            DistanceManager.MoveToInitialPlace(Stat.MovementSpeed);
        }
        public override float GetAttackValueOffset()
        {
            return LastPerfectionRate;
        }
        public override float GetDefenceValueOffset()
        {
            return LastPerfectionRate;
        }
        public override void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new System.ArgumentException("Damage cannot be less than zero for Patapons. Use Group.HealAllINGroup() or HealAlone() to heal.");
            }
            base.TakeDamage(damage);
            Group.UpdateHitPoint(this);
        }
        /// <summary>
        /// DON'T CALL THIS METHOD SEPARATELY WITHOUT <see cref="Display.PataponsHitPointDisplay.Refresh"/>. CALL ONLY IN <see cref="PataponGroup"/>. Otherwise heal status won't displayed!
        /// </summary>
        internal void Heal(PataponGroup sender, int amount)
        {
            if (sender != Group) return;
            CurrentHitPoint = Mathf.Min(CurrentHitPoint + amount, Stat.HitPoint);
        }
        public void HealAlone(int amount)
        {
            CurrentHitPoint = Mathf.Min(CurrentHitPoint + amount, Stat.HitPoint);
            Group.RefreshDisplay();
        }
        public override void OnAttackHit(Vector2 point, int damage)
        {
            base.OnAttackHit(point, damage);
            //General group effect
            if (IsGeneral && _data.Type == Class.ClassType.Tatepon && LastSong == CommandSong.Ponpon && Charged)
            {
                Group.HealAllInGroup((int)(damage * 0.1f));
            }
        }
        private void Update()
        {
            if (PataponsManager.IsMovingForward && !StatusEffectManager.CanContinue)
            {
                //Go back lol.
                transform.position -= Group.Manager.Steps * Vector3.right * Time.deltaTime;
            }
        }
    }
}
