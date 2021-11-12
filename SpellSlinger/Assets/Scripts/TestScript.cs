using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TestScript : MonoBehaviour
    {
        public GameObject spellPrefab;

        public GameObject spell;

        int counter;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                CreateSpell();

            if (Input.GetKeyDown(KeyCode.Space))
                CastSpell();
        }

        private void CreateSpell()
        {
            if (spell == null)
            {
                spell = Instantiate(spellPrefab, transform);
                spell.name = $"Spell {counter}";
                counter++;
            }
        }

        private void CastSpell()
        {
            if (spell != null)
            {
                spell.GetComponent<Spell>().CastSpell();
                spell = null;
            }
        }
    }
}
