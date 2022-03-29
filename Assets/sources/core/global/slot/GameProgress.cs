using UnityEngine;

namespace PataRoad.Core.Global.Slots
{
    [System.Serializable]
    public class GameProgress
    {
        [SerializeField]
        private bool _isGongOpen;
        public bool IsGongOpen
        {
            get => _isGongOpen;
            set => _isGongOpen = value;
        }
        [SerializeField]
        private bool _isGemMinigameOpen;
        public bool IsGemMinigameOpen
        {
            get => _isGemMinigameOpen;
            set => _isGemMinigameOpen = value;
        }
        [SerializeField]
        private bool _isEquipmentMinigameOpen;
        public bool IsEquipmentMinigameOpen
        {
            get => _isEquipmentMinigameOpen;
            set => _isEquipmentMinigameOpen = value;
        }
        [SerializeField]
        private bool _isRareponOpen;
        public bool IsRareponOpen
        {
            get => _isRareponOpen;
            set => _isRareponOpen = value;
        }
        [SerializeField]
        private bool _isSummonOpen;
        public bool IsSummonOpen
        {
            get => _isSummonOpen;
            set => _isSummonOpen = value;
        }
        [SerializeField]
        private bool _isMusicOpen;
        public bool IsMusicOpen
        {
            get => _isMusicOpen;
            set => _isMusicOpen = value;
        }
        [SerializeField]
        private bool _isExchangeOpen;
        public bool IsExchangeOpen
        {
            get => _isExchangeOpen;
            set => _isExchangeOpen = value;
        }

        public bool IsOpen(GameProgressType type) => type switch
        {
            GameProgressType.Gong => _isGongOpen,
            GameProgressType.GemMinigame => _isGemMinigameOpen,
            GameProgressType.EquipmentMinigame => _isEquipmentMinigameOpen,
            GameProgressType.Rarepon => _isRareponOpen,
            GameProgressType.Summon => _isSummonOpen,
            GameProgressType.Music => _isMusicOpen,
            GameProgressType.Exchange => _isExchangeOpen,
            _ => throw new System.NotImplementedException()
        };
    }
}
