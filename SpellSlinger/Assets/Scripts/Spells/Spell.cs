using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    { 
        private SpellType _type;
        public SpellType Type => _type;

        // Cache the collider of this spell
        private SphereCollider _collider;

        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float _spellLifeSpanInSeconds = 10.0f;
        private bool fired = false;

        // Cache the Camera forward vector on X and Z axes
        // Use to aim the spell towards the center view of the screen (Camera)
        private Vector3 forward;

        #region UNITY Methods
        private void Awake()
		{
            _collider = GetComponent<SphereCollider>();
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1);
		}

		private void Update()
		{
            // If not fired yet, update the forward vector based on the camera forward vector
            if (!fired) {
                forward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            } else {
                _collider.radius += speed * Time.deltaTime / 50.0f;
                transform.Translate(forward * speed / 100.0f, Space.World);
            }
		}
        #endregion

        #region Other Methods
        public void SetType(SpellType type)
        {
            _type = type;
            gameObject.GetComponent<MeshRenderer>().material = type.Material;
        }

        /// <summary>
        /// Dettaches the spell from parent and fires it.
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
        #endregion
    }
}
