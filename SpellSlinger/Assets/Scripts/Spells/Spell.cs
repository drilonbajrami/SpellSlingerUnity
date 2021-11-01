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

		private void Start()
		{
            rb = GetComponent<Rigidbody>();
		}

        public void SetType(ElementalProperties properties) => properties = new ElementalProperties(properties);

        public void CastSpell()
        {
            transform.parent = null;
            rb.AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
        }
    }
}
