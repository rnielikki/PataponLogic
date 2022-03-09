using PataRoad.Core.Character.Class;
using System.Collections.Generic;
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

        private static Dictionary<ClassType, (GameObject general, GameObject patapon)> _pataponObjects { get; }
            = new Dictionary<ClassType, (GameObject, GameObject)>();

        /// <summary>
        /// Generates Patapons automatically on mission.
        /// </summary>
        /// <param name="patapons">Array of Patapon types to create.</param>
        /// <param name="manager"><see cref="PataponManager"/> to reference. Also its transform is used to set as groups' parent.</param>
        internal static void Generate(IEnumerable<ClassType> patapons, PataponsManager manager)
        {
            _pataponGroupIndex = 0;
            _sortingLayerIndex = 0;
            foreach (var patapon in patapons)
            {
                AddPataponGroupInstance(patapon, manager.transform, manager, true);
            }
        }
        /// <summary>
        /// Generates every Patapons automatically outside mission.
        /// </summary>
        /// <param name="parent">Parent of the group objects.</param>
        /// <returns>Group dictionary.</returns>
        internal static Dictionary<ClassType, GameObject> Generate(Transform parent, IEnumerable<ClassType> classes)
        {
            _pataponGroupIndex = 0;
            _sortingLayerIndex = 0;
            Dictionary<ClassType, GameObject> pataponGroups = new Dictionary<ClassType, GameObject>();
            foreach (var pataponClass in classes)
            {
                var group = AddPataponGroupInstance(pataponClass, parent, null, false);
                pataponGroups.Add(pataponClass, group);
                group.SetActive(false);
            }
            return pataponGroups;
        }
        public static GameObject GetGeneralOnlyPataponGroupInstance(ClassType classType, Transform parent, PataponsManager manager)
        {
            return AddPataponGroupInstance(classType, parent, manager, true, true);
        }

        private static GameObject AddPataponGroupInstance(ClassType classType, Transform parent, PataponsManager manager, bool onMission, bool generalOnly = false)
        {
            var group = new GameObject("PataponGroup");
            group.transform.parent = parent;

            if (onMission)
            {
                var groupScript = group.AddComponent<PataponGroup>();
                groupScript.ClassType = classType;

                AddPataponsInstance(classType, group.transform, onMission, generalOnly);

                groupScript.Init(manager);
                group.transform.localPosition = Vector2.zero + (_pataponGroupIndex * PataponEnvironment.GroupDistance * Vector2.left);
            }
            else
            {
                AddPataponsInstance(classType, group.transform, onMission, generalOnly);
            }
            _pataponGroupIndex++;
            return group;
        }

        private static void AddPataponsInstance(ClassType classType, Transform attachTarget, bool onMission, bool generalOnly = false)
        {
            GameObject general;
            GameObject patapon;

            var generalOffset = onMission ? 0 : 1;
            var idleDistance = PataponEnvironment.PataponIdleDistance;
            if (!onMission) idleDistance *= 1.5f;

            (general, patapon) = LoadResource(classType);
            if (generalOnly)
            {
                Attach(Object.Instantiate(general, attachTarget), 0);
                return;
            }

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

                ponInstance.transform.localPosition = Vector2.zero + (((offset * idleDistance) + generalOffset) * Vector2.left);
            }
        }
        private static (GameObject general, GameObject patapon) LoadResource(ClassType classType)
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
