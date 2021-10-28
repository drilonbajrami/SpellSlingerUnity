using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    {
        private Rigidbody rb;

		private void Start()
		{
            rb = GetComponent<Rigidbody>();
		}

		public void CastSpell()
        {
            transform.parent = null;
            rb.AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
        }
    }
}
