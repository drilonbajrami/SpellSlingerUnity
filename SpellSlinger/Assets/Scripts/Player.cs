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
            SpellCrafter.CraftSpell += OnCreateSpell;
            CastGesture.PoseForm += OnCastSpell;
		}

		private void Update()
		{
            if (currentSpell != null)
                currentSpell.transform.position = hand.transform.position;
		}

		public void OnCreateSpell(object source, SpellType spellType)
        {
            if (spellType != null)
            {
                currentSpell = Instantiate(spellPrefab);
                currentSpell.GetComponent<Spell>().SetType(spellType);
            }
        }

        public void OnCastSpell(object source, EventArgs e)
        {
            if (currentSpell != null)
                CastSpell();
        }

        private void CastSpell()
        {
            currentSpell.GetComponent<Spell>().CastSpell();
            currentSpell = null;
        }
    }
}
