using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level42 : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Character.Patapons.PataponsManager.AllowedToGoForward = false;
            Character.Patapons.PataponsManager.Current.ForceZoomOut();
        }
    }
}