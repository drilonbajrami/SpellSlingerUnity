using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn Point")]
        [SerializeField] private Transform spawnPointParent;
        [SerializeField] private GameObject spawnPointPrefab;
        private List<GameObject> _spawnPoints = new List<GameObject>();

        [Space(10)]

        [Header("Enemy Spawner")]     
        [SerializeField] private List<EnemyType> _enemyTypes = new List<EnemyType>();
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform enemyParent;

        public float spawnRate = 5.0f;
        private float _spawnTime = 0.0f;

        private void Start()
        {
            foreach(Transform spawnPoint in spawnPointParent) _spawnPoints.Add(spawnPoint.gameObject);
        }

        #region Spawn Methods
        void Update()
        {
            _spawnTime += Time.deltaTime;

            if (_spawnTime > spawnRate)
            {
                SpawnEnemy();
                _spawnTime = 0.0f;
            }
        }

        /// <summary>
        /// Spawns an enemy and parents it to the Enemy GO transform.
        /// </summary>
        void SpawnEnemy()
        {
            GameObject enemy = Instantiate(enemyPrefab, 
                                           _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position, 
                                           Quaternion.Euler(0, 180, 0),
                                           enemyParent);
            enemy.GetComponent<Enemy>().SetType(_enemyTypes[Random.Range(0, _enemyTypes.Count)]);
        }
        #endregion

        // These methods are called in the SpawnerEditor script on inspector buttons clicked.
        #region Spawn Point Methods
        /// <summary>
        /// Spawns a spawn point and returns it.
        /// </summary>
        public GameObject AddSpawnPoint()
        {
            Vector3 randPos = new Vector3(Random.Range(transform.position.x - 5f, transform.position.x + 5f),  // x
                                          1,                                                                   // y
                                          Random.Range(transform.position.z - 5f, transform.position.z + 5f)); // z

            GameObject point = Instantiate(spawnPointPrefab, randPos, Quaternion.identity, spawnPointParent);
            return point;
        }

        /// <summary>
        /// Remove scene selected spawn point.
        /// </summary>
        /// <param name="selected"></param>
        public void RemoveSelectedPoint(GameObject selected)
        {
            if(selected.GetComponent<SpawnPoint>() != null) DestroyImmediate(selected);   
        }

        public void AddSelectedPoint(GameObject selected)
        {
            if (selected.GetComponent<SpawnPoint>() != null) DestroyImmediate(selected);
        }

        /// <summary>
        /// Clear all spawn points.
        /// </summary>
        public void ClearAllPoints()
        {
            foreach (Transform point in spawnPointParent) DestroyImmediate(point.gameObject);
        }
        #endregion
    }
}