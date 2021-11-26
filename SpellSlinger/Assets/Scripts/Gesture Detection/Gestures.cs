using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpellSlinger
{
    public class Gestures : MonoBehaviour
	{
		[SerializeField] private bool _trackersOn = true;
		private List<Gesture> gestures = new List<Gesture>();

		private void Start()
		{
			// Cache all the available gestures
			for (int i = 0; i < transform.childCount; i++)
			{
				gestures.Add(transform.GetChild(i).gameObject.GetComponent<Gesture>());
			}

			// Set tracking toggle on/off
			foreach (Gesture gesture in gestures)
				gesture.ToggleTracking(_trackersOn);
		}

		private void OnValidate()
		{
			foreach (Gesture gesture in gestures)
				gesture.ToggleTracking(_trackersOn);
		}

		public void DisableAllGestures()
        {
			foreach (Gesture gesture in gestures) gesture.Disable();
        }

		/// <summary>
		/// Enables the specified gesture
		/// </summary>
		/// <typeparam name="T">Gesture Type</typeparam>
		public void Enable<T>() where T : Gesture => gestures.OfType<T>().FirstOrDefault().Enable();

		/// <summary>
		/// Disables the specified gesture
		/// </summary>
		/// <typeparam name="T">Gesture Type</typeparam>
        public void Disable<T>() where T : Gesture => gestures.OfType<T>().FirstOrDefault().Disable();
    }
}