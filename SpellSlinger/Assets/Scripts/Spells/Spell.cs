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

        [SerializeField] private AudioSource _hitSound;

        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float _spellLifeSpanInSeconds = 10.0f;
        private bool fired = false;

        private bool wobbling = false;

        // Cache the Camera forward vector on X and Z axes
        // Use to aim the spell towards the center view of the screen (Camera)
        private Vector3 forward;

        #region UNITY Methods
        private void Awake()
		{
            _collider = GetComponent<SphereCollider>();
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 1);
            //transform.DOSc(new Vector3(.1f, .1f, .1f), 1);
		}

		private void Update()
		{
            // If not fired yet, update the forward vector based on the camera forward vector
            if (!fired) {
                if(!wobbling) StartCoroutine(Wobble(new Vector3(.07f, .07f, .07f), new Vector3(.1f, .1f, .1f), 1f));
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
            _hitSound.clip = type.AudioClip;
            gameObject.GetComponent<MeshRenderer>().material = type.Material;
        }

        public void PlayHitSound() => _hitSound.Play();

        /// <summary>
        /// Dettaches the spell from parent and fires it.
        /// </summary>
        public void CastSpell()
        {
            transform.parent = null;
            fired = true;
            StopAllCoroutines();
            wobbling = false;
            transform.DOScale(new Vector3(2,2,2), 5);
            Destroy(gameObject, _spellLifeSpanInSeconds);
        }

        private IEnumerator Wobble(Vector3 startSize, Vector3 endSize, float seconds)
        {
            wobbling = true;
            transform.DOScale(endSize, seconds);
            yield return new WaitForSeconds(seconds);
            transform.DOScale(startSize, seconds);
            yield return new WaitForSeconds(seconds);
            wobbling = false;
        }
        #endregion
    }
}
