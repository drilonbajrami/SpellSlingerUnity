using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
    [RequireComponent(typeof(Player))]
    public class SpellCreator : MonoBehaviour
    {
        private GameObject spell;
        public GameObject spellPrefab;
        public GameObject spellHolder;

        // Start is called before the first frame update
        void Start()
        {
            SpellCrafter.CraftSpell += OnCreateSpell;
            CastGesture.PoseForm += OnCastSpell;
        }

        private void Update()
        {
            if (spell != null)
                spell.transform.position = spellHolder.transform.position;
        }

        public void OnCreateSpell(object source, SpellType spellType)
        {
            if (spellType != null && spell == null)
            {
                spell = Instantiate(spellPrefab);
                spell.GetComponent<Spell>().SetType(spellType);
            }
        }

        public void OnCastSpell(object source, EventArgs e)
        {
            if (spell != null)
            {
                spell.GetComponent<Spell>().CastSpell();
                spell = null;
            }
        }
    }
}
