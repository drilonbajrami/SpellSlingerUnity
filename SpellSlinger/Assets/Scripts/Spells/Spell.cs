using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    {
        private ElementalProperties properties;
        public ElementalProperties Properties { get { return properties; } }

        private Rigidbody rb;
        private SphereCollider collider;

        public float speed = 5.0f;

        bool fired = false;

        float radius;

		private void Awake()
		{
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<SphereCollider>();
            radius = collider.radius;
		}

		private void Update()
		{
            if (fired)
            {
                radius += speed * Time.deltaTime / 50.0f;
                collider.radius = radius;

                transform.Translate(transform.forward * speed / 100.0f, Space.World);
            }
		}

		public void SetType(ElementalProperties properties) => properties = new ElementalProperties(properties);

        public void CastSpell()
        {
            transform.parent = null;
            fired = true;
            StartCoroutine(LifeSpan());
        }

        private IEnumerator LifeSpan()
        {
            yield return new WaitForSeconds(15f);
            Destroy(this.gameObject);
        }
    }
}
