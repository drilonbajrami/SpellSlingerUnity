using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSlinger
{
	public class SpellHint : MonoBehaviour
	{
		// Store the element type of the spell
		private Element _element;

		// Background sign for the element
		private Image _backgroundImg;

		// ASL sign letter 
		private Image _signLetterImg;

		// Keep track of the current element index
		// And of how many elements are available
		private int _currentElementIndex = 0;
		private int _numberOfElements;

		// Store the spell element type name and current letter index
		private char[] _letters;
		private int _currentLetterIndex = 0;

		private void Start()
		{
			// Store the number of element types
			// Need to loop through the elements when using swipe behaviour
			_numberOfElements = Enum.GetNames(typeof(Element)).Length;

			// Set the current element type index to 0
			_element = (Element)_currentElementIndex;

			// Get the current element type name into an char array so we can loop through it
			_letters = _element.ToString().ToCharArray();

			// Cache the Image components for the element sign background and letter sign
			_backgroundImg = GetComponent<Image>();
			_signLetterImg = transform.GetChild(0).gameObject.GetComponent<Image>();

			// Set default bg and first sign letter of the current element
			_backgroundImg.sprite = SpriteLibrary.Elements[_element.ToString()];
			_signLetterImg.sprite = SpriteLibrary.Alphabet[_letters[_currentLetterIndex]];

			// Listen to swipe gestures
			SwipeGesture.PoseForm += OnSwipe;
		}

		private void OnSwipe(object source, EventArgs e)
		{
			NextElement();
		}

		/// <summary>
		/// Cache the current spell type.
		/// </summary>
		/// <param name="type"></param>
		public void SetCurrentSpellElement(Element element)
		{
			// Once we know the spell we are crafting, stop listening to swipe gestures
			SwipeGesture.PoseForm -= OnSwipe;

			// Update the current element type based on the given spell being crafted
			_element = element;
			_letters = _element.ToString().ToCharArray();

			UpdateElementBackground();
			// When the element type of the spell is known, skip to the second letter in the name
			NextSignLetter();
		}

		/// <summary>
		/// Reset everything related to the spell hint panel
		/// </summary>
		public void ResetPanel()
		{
			SwipeGesture.PoseForm += OnSwipe;
			_currentElementIndex = 0;
			_currentLetterIndex = 0;
			UpdateElementBackground();
			UpdateLetterSign();
		}

		/// <summary>
		/// Show next element.
		/// </summary>
		private void NextElement()
		{
			// Increment element index counter
			_currentElementIndex++;

			// If counter of bounds, reset it to 0
			if (_currentElementIndex >= _numberOfElements)
				_currentElementIndex = 0;

			// Assign new element type and update background and sign images
			_element = (Element)_currentElementIndex;
			UpdateElementBackground();
		}

		/// <summary>
		/// Show next sign letter.
		/// </summary>
		public void NextSignLetter()
		{
			// Increment letter index counter
			_currentLetterIndex++;

			// If counter of bounds, reset it to 0
			if (_currentLetterIndex >= _letters.Length)
				_currentLetterIndex = 0;

			UpdateLetterSign();
		}

		/// <summary>
		/// Update the background image regarding the current element selected.
		/// </summary>
		private void UpdateElementBackground()
		{
			_backgroundImg.sprite = SpriteLibrary.Elements[_element.ToString()];
			_letters = _element.ToString().ToCharArray();
			UpdateLetterSign();
		}

		/// <summary>
		/// Update the current letter sign image regarding the current letter being shown.
		/// </summary>
		private void UpdateLetterSign()
		{
			_signLetterImg.sprite = SpriteLibrary.Alphabet[_letters[_currentLetterIndex]];
		}
	}
}
