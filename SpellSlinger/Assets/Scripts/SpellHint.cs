using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSlinger
{
	public class SpellHint : MonoBehaviour
	{
		private Element _element;
		private Image background;
		private Image signLetter;

		private char[] letters;
		private int currentLetterIndex = 0;

		private int currentElementIndex = 0;
		private int numberOfElements;

		private SpellType spellType;

		private void Start()
		{
			numberOfElements = Enum.GetNames(typeof(Element)).Length;
			_element = (Element)currentElementIndex;

			letters = _element.ToString().ToCharArray();
			background = GetComponent<Image>();
			signLetter = transform.GetChild(0).gameObject.GetComponent<Image>();

			background.sprite = SpriteLibrary.Elements[_element.ToString()];
			signLetter.sprite = SpriteLibrary.Alphabet[letters[currentLetterIndex]];

			SwipeGesture.PoseEvent += Swipe;
		}

		public void Swipe(object source, EventArgs e)
		{
			Refresh();
		}

		public void SetCurrentSpellElement(SpellType type)
		{
			SwipeGesture.PoseEvent -= Swipe;
			spellType = type;
			letters = _element.ToString().ToCharArray();
			NextSignLetter();
		}

		public void ResetPanel()
		{
			SwipeGesture.PoseEvent += Swipe;
			spellType = null;
			currentElementIndex = 0;
			currentLetterIndex = 0;
			UpdateElementBackground();
			UpdateLetterSign();
		}

		public void Refresh()
		{
			if (spellType != null)
				NextSignLetter();
			else
				NextElement();
		}

		public void NextElement()
		{
			currentElementIndex++;

			if (currentElementIndex >= numberOfElements)
				currentElementIndex = 0;

			_element = (Element)currentElementIndex;
			UpdateElementBackground();
			UpdateLetterSign();
		}

		public void NextSignLetter()
		{
			currentLetterIndex++;

			if (currentLetterIndex >= letters.Length)
				currentLetterIndex = 0;

			UpdateLetterSign();
		}

		private void UpdateElementBackground()
		{
			background.sprite = SpriteLibrary.Elements[_element.ToString()];
			letters = _element.ToString().ToCharArray();
		}

		private void UpdateLetterSign()
		{
			signLetter.sprite = SpriteLibrary.Alphabet[letters[currentLetterIndex]];
		}
	}
}
