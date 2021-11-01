using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Player : MonoBehaviour
    {
        public GameObject spellPrefab;
        private GameObject currentSpell;

        public GameObject hand;

		private void Start()
		{
            GestureReceiver.CraftedSpellEvent += OnCreateSpell;
            GestureReceiver.CastSpellEvent += OnCastSpell;
		}

		public void OnCreateSpell(object source, SpellType spellType)
        {
            currentSpell = Instantiate(spellPrefab, hand.transform);
        }

        public void OnCastSpell(object source, EventArgs e)
        {
            currentSpell.GetComponent<Spell>().CastSpell();
            currentSpell = null;
        }
    }
}
