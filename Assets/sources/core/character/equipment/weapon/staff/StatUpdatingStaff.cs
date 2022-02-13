using System.Collections.Generic;
using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    abstract class StatUpdatingStaff : MonoBehaviour, IStaffActions
    {
        SmallCharacter _holder;
        bool _isValidPatapon;
        private Patapons.PataponsManager _manager;
        [SerializeField]
        GameObject _prefabForActivation;
        private readonly List<Animator> _activationPrefabs = new List<Animator>();
        private bool _performedInThisTurn;
        public void Initialize(SmallCharacter holder)
        {
            _isValidPatapon = holder is Patapons.Patapon;
            if (!_isValidPatapon) return;
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
                        var prefab = Instantiate(_prefabForActivation, patapon.RootTransform);
                        _activationPrefabs.Add(prefab.GetComponent<Animator>());
                        prefab.gameObject.SetActive(false);
                        InitEach(patapon);
                    }
                }
                Rhythm.Command.TurnCounter.OnTurn.AddListener(() =>
                {
                    if (Rhythm.Command.TurnCounter.IsPlayerTurn)
                    {
                        _performedInThisTurn = false;
                        OnPerformEnd();
                    }
                });
            }
        }
        protected virtual void InitEach(Patapons.Patapon patapon)
        {
        }
        protected virtual void OnPerformEnd() { }
        protected void PerformAction()
        {
            if (!_isValidPatapon || _performedInThisTurn) return;
            _performedInThisTurn = true;
            foreach (var pataponGroup in _manager.Groups)
            {
                foreach (var patapon in pataponGroup.Patapons)
                {
                    PerformActionEach(patapon);
                }
            }
            foreach (var anim in _activationPrefabs)
            {
                if (anim?.gameObject == null) continue;
                anim.gameObject.SetActive(true);
                anim.Play("Flash");
            }
        }
        protected abstract void PerformActionEach(Patapons.Patapon patapon);
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
