using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    public class Level40 : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            //~~ preventing collection change ~~ 
            foreach (var pon in Character.Patapons.PataponsManager.Current.Patapons.ToArray())
            {
                if (pon.ClassData.IsFlyingUnit)
                {
                    pon.BeEaten();
                }
            }
        }
    }
}