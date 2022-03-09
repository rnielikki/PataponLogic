using UnityEngine;

namespace PataRoad.SceneLogic.Minigame
{
    class MinigameDrumGrid : MonoBehaviour
    {
        [SerializeField]
        private MinigameDrumAction _pataImage;
        [SerializeField]
        private MinigameDrumAction _ponImage;
        [SerializeField]
        private MinigameDrumAction _donImage;
        [SerializeField]
        private MinigameDrumAction _chakaImage;

        private MinigameDrumAction _current;

        System.Collections.Generic.Dictionary<MinigameDrumType, MinigameDrumAction> _drumMap;

        private void Start()
        {
            _drumMap = new System.Collections.Generic.Dictionary<MinigameDrumType, MinigameDrumAction>()
            {
                { MinigameDrumType.Pata, _pataImage},
                { MinigameDrumType.Pon, _ponImage},
                { MinigameDrumType.Chaka, _chakaImage},
                { MinigameDrumType.Don, _donImage},
            };
        }

        public void Load(MinigameDrumType drum)
        {
            if (drum != MinigameDrumType.Empty)
            {
                _current = _drumMap[drum];
                _current.Appear();
            }
            else _current = null;
        }
        public void Hit(float accuracy) => _current?.Hit(accuracy);
        public void Disappear() => _current?.Disappear();
        public void ClearStatus() => _current?.ResetStatus();
    }
}
