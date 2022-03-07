using PataRoad.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace PataRoad.Core.Character.Equipments.Logic
{
    /// <summary>
    /// Displays damage *as number*. called from <see cref="DamageCalculator"/>.
    /// </summary>
    internal class DamageDisplay
    {
        private readonly ObjectPool<GameObject> _pataPool;
        private readonly ObjectPool<GameObject> _nonPataPool;

        private const int initialCount = 30;
        private const int maxCount = 100;
        internal DamageDisplay()
        {
            var pataponDisplayObject = Resources.Load<GameObject>("Characters/Display/Damage/Damage.Pon");
            var nonPataponDisplayObject = Resources.Load<GameObject>("Characters/Display/Damage/Damage.NonPon");

            _pataPool = new ObjectPool<GameObject>(
                () => InstantiateDisplayObject(pataponDisplayObject, pataponDisplayObject.name),
                (obj) => obj.SetActive(true),
                (obj) => obj.SetActive(false),
                (obj) => Object.Destroy(obj),
                true,
                initialCount,
                maxCount
                );
            InitializePool(_pataPool, initialCount);

            _nonPataPool = new ObjectPool<GameObject>(
                () => InstantiateDisplayObject(nonPataponDisplayObject, pataponDisplayObject.name),
                (obj) => obj.SetActive(true),
                (obj) => obj.SetActive(false),
                (obj) => Object.Destroy(obj),
                true,
                initialCount,
                maxCount
                );
            InitializePool(_nonPataPool, initialCount);
        }

        internal void DisplayDamage(int damage, Vector2 position, bool isPatapon, bool isCritical)
        {
            var txt = isPatapon ? _pataPool.Get() : _nonPataPool.Get();
            var txtPro = txt.GetComponentInChildren<TMPro.TextMeshPro>();
            txtPro.text = damage.ToString();
            txt.transform.position = position;
            if (isCritical)
            {
                txtPro.fontWeight = TMPro.FontWeight.Heavy;
            }
        }

        private GameObject InstantiateDisplayObject(GameObject displayObject, string ponName)
        {
            var obj = Object.Instantiate(displayObject);
            var rel = obj.AddComponent<ReleaseToPool>();
            rel.Pool = displayObject.name == ponName ? _pataPool : _nonPataPool;
            return obj;
        }

        private void InitializePool(IObjectPool<GameObject> objectPool, int count)
        {
            var objects = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                objects[i] = objectPool.Get();
            }
            foreach (var obj in objects)
            {
                objectPool.Release(obj);
            }
        }
    }
}
