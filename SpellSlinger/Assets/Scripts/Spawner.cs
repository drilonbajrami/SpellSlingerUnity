using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Spawner : MonoBehaviour
    {
        public List<GameObject> _spawnPoints;


        [SerializeField] private List<EnemyType> _enemyTypes = new List<EnemyType>();

        public GameObject enemyPrefab;

        public float spawnRate = 5.0f;
        private float _spawnTime = 0.0f;

        // Used for keeping track if recording spawn points or not in Editor mode
		[SerializeField] public bool recordingSpawnPoints = false;

		private void Start()
		{
            foreach (GameObject point in _spawnPoints)
                point.GetComponent<MeshRenderer>().enabled = false;
		}

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

        public void AddSpawnPoint(GameObject spawnPoint)
        {
            if (_spawnPoints == null)
                _spawnPoints = new List<GameObject>();

            _spawnPoints.Add(spawnPoint);
        }

        public void RemoveLastPoint()
        {
            if (_spawnPoints == null)
                return;
            else
                _spawnPoints.RemoveAt(_spawnPoints.Count - 1);
        }

        public void ClearAllPoints()
        {
            _spawnPoints.Clear();
        }
        
        public void TurnRecordingOn() => recordingSpawnPoints = true;

        public void TurnRecordingOff() => recordingSpawnPoints = false;
    }
}
