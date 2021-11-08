using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TestScript : MonoBehaviour
    {
        SpellHint hint;
        // Start is called before the first frame update
        void Start()
        {
            hint = GetComponent<SpellHint>();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
                hint.NextSignLetter();
        }
    }
}
