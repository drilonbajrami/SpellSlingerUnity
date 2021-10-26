using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Enemy : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

/*
 public class Enemy : MonoBehaviour
{
    private Element _type;
    private Element _weakness;
    private float _enemyHealth;

    public Element GetElementType() => _type;
    public string GetElementTypename() => _type.ToString();

    private NavMeshAgent _agent;

    private void Start()
    {
        _enemyHealth = 100.0f;
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
    }

    public void SetType(EnemyType enemyType)
    {
        _type = enemyType.type;
        _weakness = enemyType.weakness;
        gameObject.GetComponent<Renderer>().material.color = enemyType.color;
    }

    public void TakeDamage(Spell spell)
    {
        if (spell.GetElementType() == _weakness)
            _enemyHealth -= 100.0f;
        else
            _enemyHealth -= 100.0f;

        if (_enemyHealth <= 0.0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DangerLine"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(25.0f);
            Destroy(gameObject);
        }
    }
}

[CreateAssetMenu(fileName = "Enemy Type", menuName = "EnemyType", order = 1)]
public class EnemyType : ScriptableObject
{
    public Element type;
    public Color color;
    public Element weakness;
}

public class Letter : MonoBehaviour
{
    private void Start()
    {
        SpellCaster.OnLetterSpelled += (letter) => CheckIfSpelled(letter);
        SpellCaster.SpellFailed += () => SpellLetter(false);
    }

    private void CheckIfSpelled(string letter)
    {
        if (letter == gameObject.name)
            SpellLetter(true);
    }

    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SpellLetter(bool a)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(a);
    }
}

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : ScriptableObject
{
    public Element type;
    public Color color;

    public Element GetElementType() => type;
    public string GetElementTypeName() => type.ToString();
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<EnemyType> typesOfEnemies;

    public float spawnRate = 5.0f;
    private float _spawnTime = 0.0f;
   
    void Update()
    {
        _spawnTime += Time.deltaTime;

        if(_spawnTime > spawnRate)
        {
            SpawnEnemy();
            _spawnTime = 0.0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.Euler(0, 180, 0));
        enemy.GetComponent<Enemy>().SetType(typesOfEnemies[Random.Range(0, typesOfEnemies.Count)]);
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), 0.25f, Random.Range(transform.position.z - 5.0f, transform.position.z + 5.0f));
    }
}
 */
