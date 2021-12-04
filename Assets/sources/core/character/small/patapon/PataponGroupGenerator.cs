using System.Linq;
using UnityEngine;

namespace PataRoad.Core.Character.Patapons
{
    /// <summary>
    /// Generates All Patapons from class type, including generals and groups.
    /// </summary>
    internal static class PataponGroupGenerator
    {
        const string _pataponPrefabPath = "Characters/Patapons/Prefabs/";
        const string _pataponGeneralPrefabPath = _pataponPrefabPath + "Generals/";
        private static int _pataponGroupIndex;
        private static int _sortingLayerIndex;

        private static System.Collections.Generic.Dictionary<Class.ClassType, (GameObject general, GameObject patapon)> _pataponObjects { get; }
            = new System.Collections.Generic.Dictionary<Class.ClassType, (GameObject, GameObject)>();

        /// <summary>
        /// Generates Patapons automatically on mission.
        /// </summary>
        /// <param name="patapons">Array of Patapon types to create.</param>
        /// <param name="manager"><see cref="PataponManager"/> to reference. Also its transform is used to set as groups' parent.</param>
        internal static void Generate(System.Collections.Generic.IEnumerable<Class.ClassType> patapons, PataponsManager manager)
        {
            _pataponGroupIndex = 0;
            _sortingLayerIndex = 0;
            foreach (var patapon in patapons)
            {
                AddPataponGroupInstance(patapon, manager.transform, manager, true);
            }
        }
        /// <summary>
        /// Generates Patapons automatically outside mission.
        /// </summary>
        /// <param name="patapons">Array of Patapon types to create.</param>
        internal static void Generate(System.Collections.Generic.IEnumerable<Class.ClassType> patapons, Transform parent)
        {
            _pataponGroupIndex = 0;
            _sortingLayerIndex = 0;
            foreach (var patapon in patapons)
            {
                AddPataponGroupInstance(patapon, parent, null, false);
            }
        }
        private static void AddPataponGroupInstance(Class.ClassType classType, Transform parent, PataponsManager manager, bool onMission)
        {
            var group = new GameObject("PataponGroup");
            group.transform.parent = parent;
            var distance = PataponEnvironment.GroupDistance;

            if (onMission)
            {
                var groupScript = group.AddComponent<PataponGroup>();
                groupScript.ClassType = classType;
                AddPataponsInstance(classType, group.transform, onMission);
                groupScript.Init(manager);
            }
            else
            {
                AddPataponsInstance(classType, group.transform, onMission);
                distance *= 1.5f;
            }
            group.transform.localPosition = Vector2.zero + _pataponGroupIndex * distance * Vector2.left;
            _pataponGroupIndex++;
        }

        private static void AddPataponsInstance(Class.ClassType classType, Transform attachTarget, bool onMission)
        {
            GameObject general;
            GameObject patapon;

            var generalOffset = onMission ? 0 : 1;
            var idleDistance = PataponEnvironment.PataponIdleDistance;
            if (!onMission) idleDistance *= 1.5f;


            (general, patapon) = LoadResource(classType);

            var patapons = new GameObject[4];
            patapons[0] = Object.Instantiate(general, attachTarget);
            for (int i = 1; i < 4; i++)
            {
                patapons[i] = Object.Instantiate(patapon, attachTarget);
            }
            for (int i = 0; i < 4; i++) Attach(patapons[i], i);

            if (onMission) patapons[0].GetComponent<General.PataponGeneral>().enabled = true;

            void Attach(GameObject ponInstance, int offset)
            {
                ponInstance.GetComponent<PataponData>().IndexInGroup = offset;
                if (onMission)
                {
                    var ponComponent = ponInstance.AddComponent<Patapon>();
                    ponComponent.Index = _sortingLayerIndex;
                    ponComponent.IndexInGroup = offset;
                }
                else
                {
                    ponInstance.GetComponent<PataponData>().Init();
                }

                foreach (var renderer in ponInstance.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.sortingOrder = _sortingLayerIndex;
                }
                _sortingLayerIndex++;

                ponInstance.transform.localPosition = Vector2.zero + (offset * idleDistance + generalOffset) * Vector2.left;
            }

        }
        public static GameObject GetGeneralObject(Class.ClassType classType) => LoadResource(classType).general;
        private static (GameObject general, GameObject patapon) LoadResource(Class.ClassType classType)
        {
            if (!_pataponObjects.TryGetValue(classType, out (GameObject, GameObject) res))
            {
                string className = classType.ToString();
                return (Resources.Load<GameObject>(_pataponGeneralPrefabPath + className),
                    Resources.Load<GameObject>(_pataponPrefabPath + className));
            }
            else return res;
        }
    }
}
