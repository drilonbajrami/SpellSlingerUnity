using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpellSlinger
{
	public class SpellProgress : MonoBehaviour
	{
		[SerializeField] private Color _spelledLetterColor;
		[SerializeField] private TMP_Text _spellName;
		[SerializeField] private Slider _spellTimerBar;
		private int _currentLetterIndex = 0;

		/// <summary>
		/// Get the name of the spell to be crafted.
		/// </summary>
		/// <param name="spell"></param>
		public void SpellToCraft(SpellType spell)
		{
			_currentLetterIndex = 0;
			_spellName.text = spell.Properties.GetElementTypeName();
			_spellTimerBar.value = 0.0f;
			HighlightLetter();
		}

		/// <summary>
		/// Highlight the next spelled letter.
		/// </summary>
		public void HighlightLetter()
		{
			_spellName.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_spelledLetterColor)}>{_spellName.text.Substring(0, _currentLetterIndex + 1)}</color>{_spellName.text.Substring(_currentLetterIndex + 1)}";
			_currentLetterIndex++;
		}

		/// <summary>
		/// Update spell timer progress bar
		/// </summary>
		/// <param name="percentage"></param>
		public void UpdateSpellTimer(float percentage)
		{
			_spellTimerBar.value = percentage;
		}

		public void OnDisable()
		{
			_spellName.text = string.Empty;
		}
	}
}
