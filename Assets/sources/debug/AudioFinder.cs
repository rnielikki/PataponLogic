/*
using UnityEngine;

namespace PataRoad.AppDebug
{
    public class AudioFinder : MonoBehaviour
    {
        private void OnValidate()
        {
            foreach (var obj in Resources.FindObjectsOfTypeAll<AudioSource>())
            {
                if (!obj.gameObject.activeSelf || !obj.gameObject.activeInHierarchy)
                    Debug.Log(obj.tag + " " + obj.name, obj.gameObject);
            }
        }
    }
}
*/