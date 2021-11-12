using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class DrawLine : MonoBehaviour
    {
        LineRenderer line;
        // Start is called before the first frame update
        void Start()
        {
			line = GetComponent<LineRenderer>();

            if(line != null)
			    line.positionCount = 2;
		}

        // Update is called once per frame
        void Update()
        {
            if (line != null)
            {
                Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
                Vector3 position = new Vector3(transform.position.x, 1, transform.position.z);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, position + forward * 100.0f);
            }
        }
    }
}
