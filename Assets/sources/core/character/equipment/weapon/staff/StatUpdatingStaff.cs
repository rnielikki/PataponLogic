using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    abstract class StatUpdatingStaff : MonoBehaviour, IStaffActions
    {
        protected Patapons.Patapon _holder { get; private set; }
        bool _isValidPatapon;
        protected Patapons.PataponsManager _manager { get; private set; }
        [SerializeField]
        protected GameObject _prefabForActivation;
        private bool _performedInThisTurn;
        public void Initialize(SmallCharacter holder)
        {
            if (holder is Patapons.Patapon patapon)
            {
                _holder = patapon;
                _isValidPatapon = true;
            }
            else return;
            _manager = GameObject.FindGameObjectWithTag("Player").GetComponent<Patapons.PataponsManager>();
            if (_manager == null)
            {
                _isValidPatapon = false;
            }
        }
        private void Start()
        {
            if (_isValidPatapon)
            {
                foreach (var pataponGroup in _manager.Groups)
                {
                    foreach (var patapon in pataponGroup.Patapons)
                    {
                        InitEach(patapon);
                    }
                }
                Rhythm.Command.TurnCounter.OnTurn.AddListener(() =>
                {
                    if (Rhythm.Command.TurnCounter.IsPlayerTurn)
                    {
                        _performedInThisTurn = false;
                    }
                });
            }
        }
        protected virtual void InitEach(Patapons.Patapon patapon)
        {
        }
        protected void PerformAction()
        {
            if (!CanPerform()) return;
            _performedInThisTurn = true;
            foreach (var pataponGroup in _manager.Groups)
            {
                foreach (var patapon in pataponGroup.Patapons)
                {
                    PerformActionEach(patapon);
                }
            }
            PerformAnimation();
        }
        protected virtual void PerformAnimation() { }
        protected abstract void PerformActionEach(Patapons.Patapon patapon);
        protected bool CanPerform() => _isValidPatapon && !_performedInThisTurn;
        public virtual void NormalAttack()
        {
            PerformAction();
        }
        public virtual void ChargeAttack()
        {
            PerformAction();
        }
        public virtual void Defend()
        {
            PerformAction();
        }
        public virtual void SetElementalColor(Color color)
        {
            //should do nothing I guess?
        }
    }
}
