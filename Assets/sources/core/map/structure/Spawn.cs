using UnityEngine;

namespace PataRoad.Core.Character
{
    public class Spawn : Structure
    {
        [SerializeField]
        GameObject[] _spawnTarget;
        [SerializeField]
        float _spawnInterval;
        [SerializeField]
        int _spawnAmount;
        int _currentCount;

        int _index;
        private void Start()
        {
            StartCoroutine(SpawnEnemy());
        }
        protected System.Collections.IEnumerator SpawnEnemy()
        {
            while (!IsDead)
            {
                yield return new WaitUntil(() => _currentCount < _spawnAmount);
                while (_currentCount < _spawnAmount)
                {
                    yield return new WaitForSeconds(_spawnInterval);
                    var spawnedObject = Instantiate(_spawnTarget[_index], transform.parent);
                    spawnedObject.transform.position = transform.position;

                    var spawned = spawnedObject.GetComponent<Hazorons.Hazoron>();
                    spawned?.OnAfterDeath?.AddListener(() => _currentCount--);
                    _currentCount++;

                    _index = (_index + 1) % _spawnTarget.Length;
                }
            }
        }
        public override void Die()
        {
            StopAllCoroutines();
            base.Die();
        }
    }
}
