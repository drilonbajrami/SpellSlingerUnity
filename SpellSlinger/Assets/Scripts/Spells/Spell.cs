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

        Vector3 forward;

        private void Awake()
		{
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<SphereCollider>();
            radius = collider.radius;
		}

		private void Update()
		{
            float rotation = 45 * Time.deltaTime;
            transform.Rotate(0, 0, rotation);
            

            if (!fired)
            {
                forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            }
            if (fired)
            {
                radius += speed * Time.deltaTime / 50.0f;
                collider.radius = radius;

                transform.Translate(forward * speed / 100.0f, Space.World);
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
