using UnityEngine;
using PataRoad.Core.Rhythm.Command;

namespace PataRoad.Core.Character.Patapons.General
{
    abstract class StatChangingGeneralMode : GeneralMode
    {
        protected IStatOperation _operation;
        private readonly System.Collections.Generic.List<PataponGroup> _groups = new System.Collections.Generic.List<PataponGroup>();
        private int _activeTurn;
        private int _leftActiveTurnCount;
        [SerializeField]
        private GameObject _counterTemplate;

        private bool _activatedInThisTurn;
        private bool _updatedInThisTurn;

        private TMPro.TextMeshPro _text;
        private Transform _textParent;
        private Vector3 _textAttachPosition;

        protected void Init(int activeTurn)
        {
            _activeTurn = activeTurn;
            _operation = new GeneralModeStatOperation(this);
            var general = GetComponentInParent<Patapon>();
            _textParent = general.transform.Find(general.RootName);
            _textAttachPosition = new Vector3(0, general.RendererInfo.BoundingOffset.y, 0);
        }

        public override void Activate(PataponGroup group)
        {
            if (_groups.Contains(group))
            {
                _activatedInThisTurn = true;
                return;
            }

            _groups.Add(group);
            foreach (var patapon in group.Patapons)
            {
                patapon.StatOperator.Add(_operation);
            }
            TurnCounter.OnNextTurn.AddListener(DeactivateAfterTurns);
        }

        private void DeactivateAfterTurns()
        {
            UpdateStatus(_activeTurn);
            TurnCounter.OnTurn.AddListener(CountUntilDeactive);
        }

        private void CountUntilDeactive()
        {
            if (TurnCounter.IsPlayerTurn)
            {
                _updatedInThisTurn = false;
            }
            else if (!_updatedInThisTurn)
            {
                if (!_activatedInThisTurn)
                {
                    UpdateStatus(_leftActiveTurnCount - 1);
                }
                if (_leftActiveTurnCount < 1)
                {
                    Deactivate();
                    TurnCounter.OnTurn.RemoveListener(CountUntilDeactive);
                }
                else
                {
                    _updatedInThisTurn = true;
                    _activatedInThisTurn = false;
                }
            }
        }

        private void Deactivate()
        {
            _leftActiveTurnCount = 0;
            foreach (var group in _groups)
            {
                foreach (var patapon in group.Patapons)
                {
                    patapon.StatOperator.Remove(_operation);
                }
            }
            if (_text != null)
            {
                Destroy(_text.gameObject);
                _text = null;
            }
            _activatedInThisTurn = false;
            _updatedInThisTurn = false;
            _groups.Clear();
        }
        public abstract Stat CalculateStat(Stat stat);

        public override void CancelGeneralMode()
        {
            Deactivate();
            TurnCounter.OnNextTurn.RemoveListener(DeactivateAfterTurns);
            TurnCounter.OnTurn.RemoveListener(CountUntilDeactive);
        }

        private void UpdateStatus(int count)
        {
            if (_text == null)
            {
                var inst = Instantiate(_counterTemplate, _textParent);
                _text = inst.GetComponent<TMPro.TextMeshPro>();
            }
            _text.text = count.ToString();
            _leftActiveTurnCount = count;
        }
    }
}
