using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpellSlinger
{
    public class Enemy : MonoBehaviour
    {
        // Type of enemy
        private EnemyType _type;
        public EnemyType Type => _type;

        private float _health;
        private NavMeshAgent _agent;

        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minSpeed;

        private void Start()
        {
            _health = 100.0f;
            _agent = GetComponent<NavMeshAgent>();
            _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
            _agent.speed = _maxSpeed;
        }

        public void SetType(EnemyType type)
        {
            _type = type;
            gameObject.GetComponent<MeshRenderer>().material.color = Type.Color;
        }

		public void TakeDamage(Spell spell)
        {
            Element spellElement = spell.Type.Element;

			if (spellElement == Type.Strength) _health -= 25.0f;
			else if (spellElement == Type.Element) _health -= 50.0f;
			else if (spellElement == Type.Weakness) _health -= 100.0f;

			if (_health <= 0.0f)
				Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision collision)
		{      
            if (collision.gameObject.CompareTag("Spell"))
            {
                Spell spell = collision.gameObject.GetComponent<Spell>();
                Instantiate(spell.Type.Effect, transform.position, Quaternion.identity);
                TakeDamage(spell);
                StartCoroutine(SlowDown());
            }
        }

		private IEnumerator SlowDown()
        {
            _agent.speed = _minSpeed;
            yield return new WaitForSeconds(3);
            _agent.speed = _maxSpeed;
        }
    }
}