using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
	public class HelpPanel : MonoBehaviour
	{
		private List<SpellHint> hints = new List<SpellHint>();
		private int childCount = 0;
		private int currentPanelIndex = 0;

		private SpellHint _currentSpellHintInUse;

		#region UNITY
		void Start()
		{
			childCount = gameObject.transform.childCount;

			for (int i = 0; i < childCount; i++)
			{
				SpellHint hint = gameObject.transform.GetChild(i).GetComponent<SpellHint>();
				hints.Add(hint);
				hint.gameObject.SetActive(false);
			}

			// Reset the default starting hint
			currentPanelIndex = 0;
			hints[currentPanelIndex].gameObject.SetActive(true);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				hints[currentPanelIndex].NextSignLetter();
		}

		private void OnEnable()
		{
			EnableSwipe(true);
		}

		private void OnDisable()
		{
			EnableSwipe(false);
		}
		#endregion

		private void SwipePose(object source, EventArgs e)
		{
			Next();
		}

		/// <summary>
		/// Enable swiping behaviour depending on spell crafting progress.
		/// </summary>
		/// <param name="condition"></param>
		public void EnableSwipe(bool condition)
		{
			if (condition)
				SwipeGesture.PoseEvent += SwipePose;
			else
				SwipeGesture.PoseEvent -= SwipePose;
		}

		/// <summary>
		/// Show the next tip.
		/// </summary>
		private void Next()
		{
			hints[currentPanelIndex].gameObject.SetActive(false);
			currentPanelIndex++;

			if (currentPanelIndex == childCount)
				currentPanelIndex = 0;

			hints[currentPanelIndex].gameObject.SetActive(true);
		}

		/// <summary>
		/// Reset panel to default.
		/// </summary>
		public void ResetPanel()
		{
			foreach (SpellHint hint in hints)
			{
				//hint.ResetView();
				hint.gameObject.SetActive(false);		
			}

			currentPanelIndex = 0;
			hints[currentPanelIndex].gameObject.SetActive(true);
		}

		public void SetCurrentSpellHint(SpellType spellType)
		{
			if (spellType == null)
				_currentSpellHintInUse = null;
			else
				_currentSpellHintInUse = hints.Find(a => a.gameObject.name == spellType.Properties.GetElementTypeName());
		}

		public void NextLetterHint()
		{
			//_currentSpellHintInUse.ShowNextLetterTip();
		}
	}
}