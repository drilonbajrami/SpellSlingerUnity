using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSlinger
{
    public class SpellHint : MonoBehaviour
    {
		private Image startingLetter;
		private List<Image> followingLetters;

		private int currentLetterIndex = 0;

		private void Start()
		{
			startingLetter = GetComponent<Image>();

			for (int i = 0; i < transform.childCount; i++)
			{
				followingLetters.Add(transform.GetChild(i).GetComponent<Image>());
				followingLetters[i].enabled = false;
			}
		}

		public void ShowNextLetterTip()
		{
			if (startingLetter.enabled)
			{
				startingLetter.enabled = false;
				currentLetterIndex++;
			}

			if (currentLetterIndex < transform.childCount)
			{
				followingLetters[currentLetterIndex].enabled = true;
				currentLetterIndex++;
			}
		}

		public void ResetView()
		{
			followingLetters[currentLetterIndex].enabled = false;
			startingLetter.enabled = true;
			currentLetterIndex = 0;
		}
	}
}
