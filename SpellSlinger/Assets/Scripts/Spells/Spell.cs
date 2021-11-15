using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    { 
        // Element properties
        private ElementalProperties _properties;
        public ElementalProperties Properties => _properties;

        // Cache the collider of this spell
        private SphereCollider _collider;

        // Speed of spell when fired
        public float speed = 5.0f;

        // Keep track if fired or not
        private bool fired = false;

        // Spell life span 
        [SerializeField] private float _spellLifeSpanInSeconds = 10.0f;

        // Cache the Camera forward vector on X and Z axes
        // Use to aim the spell towards the center view of the screen (Camera)
        Vector3 forward;

        private void Awake()
		{
            _collider = GetComponent<SphereCollider>();
		}

		private void Update()
		{
            // If not fired yet, update the forward vector based on the camera forward vector
            if (!fired)
            {
                forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            }
            if (fired)
            {                                                                                                                                                                                               
                _collider.radius = speed * Time.deltaTime / 50.0f;
                transform.Translate(forward * speed / 100.0f, Space.World);
            }
		}

        public void SetType(ElementalProperties properties)
        {
            _properties = new ElementalProperties(properties);
            gameObject.GetComponent<MeshRenderer>().material = properties.GetMaterial();
        }

        /// <summary>
        /// Dettached the spell from parent and fires it.
        /// </summary>
        public void CastSpell()
        {
            transform.parent = null;
            fired = true;
            StartCoroutine(LifeSpan(_spellLifeSpanInSeconds));
        }

        /// <summary>
        /// Destroy the spell after given amount of seconds have passed.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private IEnumerator LifeSpan(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(this.gameObject);
        }
    }
}
