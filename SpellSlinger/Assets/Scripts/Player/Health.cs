using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace SpellSlinger
{
    public class Health : MonoBehaviour
    {
        public static event EventHandler Death;
        public static event EventHandler<int> Damage;
        public int Lives { get; private set; }

        private bool regeneratingHealth = false;
        public float regenerationDurationInSeconds = 3f;

        private void Start() => ResetHealth();

        public void ResetHealth() => Lives = 3;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // TEST
                TakeDamage();

            if(Lives < 3 && !regeneratingHealth)
                StartCoroutine(RegenerateHealth());
        }

        public void TakeDamage()
        {
            StopAllCoroutines();
            regeneratingHealth = false;
            Lives--;
            Damage?.Invoke(this, Lives);
            if(Lives == 0) Death?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator RegenerateHealth()
        {
            regeneratingHealth = true;
            yield return new WaitForSeconds(regenerationDurationInSeconds);          
            Lives++;
            regeneratingHealth = false;
            Damage?.Invoke(this, Lives);
        }
    }
}
