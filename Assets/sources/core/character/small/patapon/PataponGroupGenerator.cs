using UnityEngine;

namespace PataRoad.Core.Character.Patapon
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

        /// <summary>
        /// Generates Patapons automatically..
        /// </summary>
        /// <param name="patapons">Array of Patapon types to create.</param>
        /// <param name="manager"><see cref="PataponManager"/> to reference. Also its transform is used to set as groups' parent.</param>
        internal static void Generate(ClassType[] patapons, PataponsManager manager)
        {
            _pataponGroupIndex = 0;
            _sortingLayerIndex = 0;
            foreach (var patapon in patapons)
            {
                AddPataponGroupInstance(patapon, manager);
            }
        }
        private static void AddPataponGroupInstance(ClassType classType, PataponsManager manager)
        {
            var group = new GameObject("PataponGroup");
            var groupScript = group.AddComponent<PataponGroup>();
            groupScript.ClassType = classType;

            AddPataponsInstance(classType.ToString(), group.transform);
            groupScript.Init(manager);

            group.transform.parent = manager.transform;
            group.transform.localPosition = Vector2.zero + _pataponGroupIndex * PataponEnvironment.GroupDistance * Vector2.left;
            _pataponGroupIndex++;
        }

        private static void AddPataponsInstance(string className, Transform attachTarget)
        {
            var generalRes = Resources.Load(_pataponGeneralPrefabPath + className) as GameObject;
            Attach(generalRes, 0);

            var ponRes = Resources.Load(_pataponPrefabPath + className) as GameObject;
            for (int i = 1; i < 4; i++)
            {
                Attach(ponRes, i);
            }

            void Attach(GameObject ponObject, int offset)
            {
                var ponInstance = Object.Instantiate(ponObject, attachTarget.transform);
                var ponComponent = ponInstance.GetComponent<Patapon>();
                ponComponent.Index = _sortingLayerIndex;
                ponComponent.IndexInGroup = offset;

                foreach (var renderer in ponComponent.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.sortingOrder = _sortingLayerIndex;
                }
                _sortingLayerIndex++;

                ponInstance.transform.localPosition = Vector2.zero + offset * PataponEnvironment.PataponIdleDistance * Vector2.left;
            }
        }
    }
}
