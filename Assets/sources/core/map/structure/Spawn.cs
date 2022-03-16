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
        private int _level;
        private int _maxLevel;

        bool _lockFromDestroying;
        protected override void Start()
        {
            base.Start();
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

                    _lockFromDestroying = true;
                    var spawnedObject = Instantiate(_spawnTarget[_index], transform.parent);
                    spawnedObject.transform.position = transform.position;

                    var spawned = spawnedObject.GetComponent<Hazorons.Hazoron>();
                    if (spawned != null)
                    {
                        spawned.OnAfterDeath.AddListener(() => _currentCount--);
                        _currentCount++;

                        var statModifier = spawned.GetComponent<Map.IHavingLevel>();
                        if (statModifier != null)
                        {
                            statModifier.SetLevel(_level, _maxLevel);
                        }
                    }
                    _index = (_index + 1) % _spawnTarget.Length;
                    _lockFromDestroying = false;
                }
            }
        }
        public override void SetLevel(int level, int absoluteMaxLevel)
        {
            base.SetLevel(level, absoluteMaxLevel);
            _level = level;
            _maxLevel = absoluteMaxLevel;
        }
        public override void Die()
        {
            StartCoroutine(WaitUntilEnd());
            System.Collections.IEnumerator WaitUntilEnd()
            {
                yield return new WaitUntil(() => !_lockFromDestroying);
                base.Die();
                StopAllCoroutines();
            }
        }
    }
}
