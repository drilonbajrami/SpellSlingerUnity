using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		[SerializeField] private bool _trackersOn = false;

		private List<Gesture> gestures = new List<Gesture>();

		private void Start()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				gestures.Add(transform.GetChild(i).gameObject.GetComponent<Gesture>());
			}

			foreach (Gesture gesture in gestures)
				gesture.TrackingOn(_trackersOn);
		}

		private void OnValidate()
		{
			foreach (Gesture gesture in gestures)
				gesture.TrackingOn(_trackersOn);
		}
	}
}