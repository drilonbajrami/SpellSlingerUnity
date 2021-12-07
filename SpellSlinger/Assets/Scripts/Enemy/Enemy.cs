using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpellSlinger
{
    public class Enemy : MonoBehaviour
    {
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

            if( _agent != null )
            {
                _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
                _agent.speed = _maxSpeed;
            }
        }

        public void SetType(EnemyType type)
        {
            _type = type;
            gameObject.GetComponent<MeshRenderer>().material.color = Type.Color;
        }

		public void TakeDamage(Spell spell)
        {
            Element spellElement = spell.Type.Element;
            float damageDealt = 0;

			if (spellElement == Type.Strength) damageDealt = 25.0f;
			else if (spellElement == Type.Element) damageDealt = 50.0f;
			else if (spellElement == Type.Weakness) damageDealt = 100.0f;

            if (damageDealt == 100)
                damageDealt = 150;

            _health -= damageDealt;
            Player.Instance.UpdateScore((int)damageDealt);
			if (_health <= 0.0f) Destroy(gameObject);
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

        /// <summary>
        /// When hit by a spell, slow down for 2 seconds.
        /// </summary>
		private IEnumerator SlowDown()
        {
            _agent.speed = _minSpeed;
            yield return new WaitForSeconds(1);
            _agent.speed = _maxSpeed;
        }
    }
}