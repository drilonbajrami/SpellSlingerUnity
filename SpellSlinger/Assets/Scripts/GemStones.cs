using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SpellSlinger
{
    public class GemStones : MonoBehaviour
    {
        private float gemStoneHP = 100;

        public static EventHandler GemStonesDestroyed;

        public void ResetHP()
        {
            gemStoneHP = 100;
        }

        public void TakeDamage(float amount)
        {
            gemStoneHP -= amount;
            if (gemStoneHP <= 0)
                GemStonesDestroyed?.Invoke(this, EventArgs.Empty);

            //Debug.Log($"Gem Stones HP: {gemStoneHP}");
        }
    }
}