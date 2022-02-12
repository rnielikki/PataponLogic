using UnityEngine;

namespace PataRoad.Core.Character.Equipments.Weapons
{
    class PanPakaHorn : Horn
    {
        private void Awake()
        {
            _forceMultiplier = 1.5f;
            _feverPonponForceMultiplier = 2;
        }
    }
}
