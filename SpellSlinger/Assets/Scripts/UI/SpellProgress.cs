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
		/// <summary>
		/// The color for a spelled letter to highlight
		/// </summary>
		[SerializeField] private Color _spelledLetterColor;

		[SerializeField] private TMP_Text _spellName;
		[SerializeField] private Slider _spellTimerBar;
		private int _currentLetterIndex = 0;

		// We cache the spell name again so we can change colors for each specific letter individually
		private string _spellNameCache;

		/// <summary>
		/// Get the name of the spell to be crafted.
		/// </summary>
		/// <param name="spell"></param>
		public void SpellToCraft(SpellType spell)
		{
			_currentLetterIndex = 0;
			_spellName.text = spell.GetElementName();
			_spellNameCache = _spellName.text;
			_spellTimerBar.value = 0.0f;

			// Highlight the first letter when we know which spell type we are crafting since we already picked the spell type
			HighlightLetter();
		}

		/// <summary>
		/// Highlight the current spelled letter.
		/// </summary>
		public void HighlightLetter()
		{
			_spellName.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_spelledLetterColor)}>{_spellNameCache.Substring(0, _currentLetterIndex + 1)}</color>{_spellNameCache.Substring(_currentLetterIndex + 1)}";
			_currentLetterIndex++;
		}

		/// <summary>
		/// Update spell timer progress bar.
		/// </summary>
		/// <param name="percentage"></param>
		public void UpdateSpellTimer(float percentage) => _spellTimerBar.value = percentage;

		/// <summary>
		/// Reset spell progress text.
		/// </summary>
		public void ResetText()
		{
			_currentLetterIndex = 0;
			_spellName.text = string.Empty;
			_spellNameCache = string.Empty;
		}

		public void OnDisable()
		{
			_spellName.text = string.Empty;
			_spellNameCache = string.Empty;
		}
	}
}
