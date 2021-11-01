using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpellSlinger
{
    public class Enemy : MonoBehaviour
    {
        private ElementalProperties properties;
        public ElementalProperties Properties { get { return properties; } }

        private float _health;
        private NavMeshAgent _agent;

        private void Start()
        {
            _health = 100.0f;
            _agent = GetComponent<NavMeshAgent>();
            _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
        }

		public void SetType(ElementalProperties properties) => properties = new ElementalProperties(properties);

		public void TakeDamage(Spell spell)
        {
            //if (spell.GetElementType() == _weakness)
            //    _enemyHealth -= 100.0f;
            //else
            //    _enemyHealth -= 100.0f;

            //if (_enemyHealth <= 0.0f)
            //    Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DangerLine"))
            {
                //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(25.0f);
                //Destroy(gameObject);
            }
        }
    }
}