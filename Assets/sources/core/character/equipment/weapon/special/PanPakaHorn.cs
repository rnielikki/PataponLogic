using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class PanPakaHorn : Horn
    {
        private void Awake()
        {
            _forceMultiplier = 3;
            _feverPonponForceMultiplier = 20;
        }
    }
}
