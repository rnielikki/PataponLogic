using UnityEngine;

namespace Core.Character.Equipment.Weapon
{
    public class Spear : WeaponObject
    {
        /// <summary>
        /// copied spear for throwing.
        /// </summary>
        private GameObject _copiedSpear;
        private void Awake()
        {
            Init();
            _copiedSpear = Resources.Load("Characters/Equipments/PrefabBase/WeaponInstance") as GameObject;
            _copiedSpear.GetComponent<WeaponInstance>().SetSprite(GetComponent<SpriteRenderer>().sprite);
        }
        public override void Attack(int times = 1)
        {
            Throw(_holder.Stat.AttackSeconds * 0.5f);
        }

        private void Throw(float delay)
        {

            StartCoroutine(ThrowWeapon());
            System.Collections.IEnumerator ThrowWeapon()
            {
                for (int i = 0; i <= delay + 0.01f; i++)
                {
                    yield return new WaitForSeconds(delay);
                    var spearForThrowing = Instantiate(_copiedSpear, transform.root.parent);
                    spearForThrowing.transform.position = transform.position;
                    spearForThrowing.transform.rotation = transform.rotation;
                    spearForThrowing.GetComponent<WeaponInstance>().Throw();
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
