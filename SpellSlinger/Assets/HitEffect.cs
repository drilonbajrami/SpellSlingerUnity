using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class HitEffect : MonoBehaviour
    {
        public static float _effectDuration = 1.0f;

        void Start()
        {
            StartCoroutine(OnSpawn());
        }

        private IEnumerator OnSpawn()
        {
            yield return new WaitForSeconds(_effectDuration);
            Destroy(gameObject);
        }
    }
}
