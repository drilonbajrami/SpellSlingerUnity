using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class CollisionCheckTest : MonoBehaviour
    {
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Spell"))
			{
				gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
			}
		}
	}
}
