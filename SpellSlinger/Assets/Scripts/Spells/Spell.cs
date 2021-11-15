using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    {

        // Element
        private ElementalProperties _properties;
        public ElementalProperties Properties => _properties;

        private SphereCollider collider;

        public float speed = 5.0f;
        private bool fired = false;
        private float radius;

        Vector3 forward;

        private void Awake()
		{
            collider = GetComponent<SphereCollider>();
            radius = collider.radius;
		}

		private void Update()
		{
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

        public void SetType(ElementalProperties properties)
        {
            this._properties = new ElementalProperties(properties);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("MainColor", properties.GetColor());
            
        }

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
