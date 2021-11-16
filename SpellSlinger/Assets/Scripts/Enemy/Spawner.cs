using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<EnemyType> _enemyTypes = new List<EnemyType>();

        public GameObject enemyPrefab;

        public float spawnRate = 5.0f;
        private float _spawnTime = 0.0f;

        void Update()
        {
            _spawnTime += Time.deltaTime;

            if (_spawnTime > spawnRate)
            {
                SpawnEnemy();
                _spawnTime = 0.0f;
            }
        }

        void SpawnEnemy()
        {
            GameObject enemy = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.Euler(0, 180, 0));
            enemy.GetComponent<Enemy>().SetType(_enemyTypes[Random.Range(0, _enemyTypes.Count)]);
        }

        Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-45f, 45f), 1f, Random.Range(70f, 85f));
        }
    }
}
