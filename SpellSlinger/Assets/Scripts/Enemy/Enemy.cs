using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpellSlinger
{
    public class Enemy : MonoBehaviour
    {
        // Element properties
        private ElementalProperties _properties;
        public ElementalProperties Properties => _properties;

        private float _health;
        private NavMeshAgent _agent;

        private void Start()
        {
            _health = 100.0f;
            _agent = GetComponent<NavMeshAgent>();
            _agent.SetDestination(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
        }

		public void SetType(ElementalProperties properties) => _properties = new ElementalProperties(properties);

		public void TakeDamage(Spell spell)
        {
            Element spellElement = spell.Properties.GetElementType();

            if (spellElement == Properties.GetStrengthElementType()) _health -= 25.0f;
            else if (spellElement == Properties.GetElementType()) _health -= 50.0f;
            else if (spellElement == Properties.GetWeaknessElementType()) _health -= 100.0f;

			if (_health <= 0.0f)
				Destroy(gameObject);
		}

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Spell"))
            {
                TakeDamage(other.gameObject.GetComponent<Spell>());
            }
        }
	}
}