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
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private Transform enemyParent;

        [Space(10)]
        [Header("Spawn Rate")]
        public float _spawnRate = 5.0f;
        private float _spawnTime = 0.0f;

        private int _enemiesToSpawn = 10;

        #region UNITY Methods
        private void Start()
        {
            foreach (Transform spawnPoint in spawnPointParent) _spawnPoints.Add(spawnPoint.gameObject);
        }

        void Update()
        {
            _spawnTime += Time.deltaTime;

            if (_spawnTime > _spawnRate && _enemiesToSpawn > 0)
            {
                SpawnEnemy();
                _enemiesToSpawn--;
                _spawnTime = 0.0f;
            }
        }

        private void OnDisable()
        {
            foreach (Transform enemy in enemyParent)
                Destroy(enemy.gameObject);
        }
        #endregion

        #region Spawn Enemy Methods
        public void ApplyGameSettings(GameSettings gameSettings)
        {
            _spawnRate = gameSettings.EnemySpawnRate;
            _enemiesToSpawn = gameSettings.EnemiesToSpawn;
        }

        void SpawnEnemy() {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
                       _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position,
                       Quaternion.Euler(0, 180, 0),
                       enemyParent);
        }
        #endregion

        // These methods are called in the SpawnerEditor script on inspector buttons clicked.
        #region Spawn Point Methods
        public GameObject AddSpawnPoint()
        {
            Vector3 randPos = new Vector3(Random.Range(transform.position.x - 5f, transform.position.x + 5f),    // x
                                          transform.position.y,                                                  // y                // y
                                          Random.Range(transform.position.z - 5f, transform.position.z + 5f)); ; // z

            GameObject point = Instantiate(spawnPointPrefab, transform.position, Quaternion.identity, spawnPointParent);
            return point;
        }

        public void RemoveSelectedPoint(GameObject selected)
        {
            if (selected.GetComponent<SpawnPoint>() != null) DestroyImmediate(selected);
        }

        public void AddSelectedPoint(GameObject selected)
        {
            if (selected.GetComponent<SpawnPoint>() != null) DestroyImmediate(selected);
        }

        public void ClearAllPoints()
        {
            foreach (Transform point in spawnPointParent) DestroyImmediate(point.gameObject);
        }
        #endregion
    }
}