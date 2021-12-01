using System;
using UnityEngine;

namespace SpellSlinger
{
    public class Health : MonoBehaviour
    {
        public static event EventHandler Death;
        public static event EventHandler<int> Damage;
        public int Lives { get; private set; }

        private void Start()
        {
            Lives = 3;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TakeDamage();
        }

        public void TakeDamage()
        {
            Lives--;
            Damage?.Invoke(this, Lives);

            if(Lives == 0)
                Death?.Invoke(this, EventArgs.Empty);
        }
    }
}
