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

        #region UNITY Methods
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

			SpellCrafter.StartCrafting += OnStartCrafting;
			SpellCrafter.CraftSpell += OnCraftSpell;
			SpellCrafter.LetterSent += OnLetterSent;
			HelpGesture.PoseForm += TogglePanel;
			SwipeGesture.PoseForm += OnSwipe;

			gameObject.SetActive(false);
		}
        #endregion

        #region Event Subscriber Methods
        private void OnStartCrafting(object sender, SpellType spellType)
		{
			if (spellType == null) ResetPanel();
			else SetCurrentSpellElement(spellType.Element);
		}

        private void OnCraftSpell(object sender, SpellType e)
        {
			ResetPanel();
			gameObject.SetActive(false);
        }

		private void OnLetterSent(object sender, EventArgs e) => NextSignLetter();

		private void TogglePanel(object sender, bool e)
		{
			if(e) {
				// Turn on hint panel and enable swipe gesture if no element selected for crafting yet
				gameObject.SetActive(e);
				if (_currentLetterIndex == 0)
					Player.Instance.Gestures.Enable<SwipeGesture>();	
            } else {
				// Disable Swipe gesture since we are closing the help/hint panel
				Player.Instance.Gestures.Disable<SwipeGesture>();
				gameObject.SetActive(e);
			}
		}

		private void OnSwipe(object source, EventArgs e) => NextElement();
        #endregion

        #region Private Methods
        /// <summary>
        /// Cache the current spell type.
        /// </summary>
        /// <param name="type"></param>
        private void SetCurrentSpellElement(Element element)
		{
			// Turn off swipe gesture since we do not need it anymore once we know the element we are crafting
			Player.Instance.Gestures.Disable<SwipeGesture>();
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
		private void ResetPanel()
		{
			_currentElementIndex = 0;
			_currentLetterIndex = 0;
			_element = (Element)_currentElementIndex;
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
		private void NextSignLetter()
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
		private void UpdateLetterSign() => _signLetterImg.sprite = SpriteLibrary.Alphabet[_letters[_currentLetterIndex]];
        #endregion
    }
}