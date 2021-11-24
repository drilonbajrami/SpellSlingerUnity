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
		private string _defaultText = "Start spelling";

        #region UNITY Methods
        private void Start()
        {
			SpellCrafter.StartCrafting += OnStartCrafting;
			SpellCrafter.CraftSpell += OnCraftSpell;
			SpellCrafter.LetterSent += OnLetterSent;

			gameObject.SetActive(false);
		}

        private void Update() => UpdateSpellTimer(SpellCrafter._craftingTimer.GetProgressPercentage());

		private void OnDisable()
		{
			_spellNameCache = string.Empty;
			ResetTextToDefault();
		}
		#endregion

		#region Event Subscriber Methods
		private void OnStartCrafting(object sender, SpellType spellType)
		{
			if (!gameObject.activeSelf)
			{
				Player.Instance.Gestures.Enable<HelpGesture>();
				gameObject.SetActive(true);
			}
			else if (spellType == null) ResetTextToDefault();
			else SpellToCraft(spellType);
		}

		private void OnCraftSpell(object sender, SpellType spellType)
		{
			Player.Instance.Gestures.Disable<HelpGesture>();
			ResetTextToDefault();
			gameObject.SetActive(false);
		}

		private void OnLetterSent(object sender, EventArgs e) => HighlightLetter();
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the name of the spell to be crafted.
        /// </summary>
        /// <param name="spell"></param>
        private void SpellToCraft(SpellType spell)
        {
            _currentLetterIndex = 0;
			_spellName.text = spell.GetElementName();
			_spellName.fontSize = 50;
			_spellName.characterSpacing = 50;
			_spellNameCache = _spellName.text;
			_spellTimerBar.value = 0.0f;

            // Highlight the first letter when we know which spell type we are crafting since we already picked the spell type
            HighlightLetter();
        }

        /// <summary>
        /// Highlight the current spelled letter.
        /// </summary>
        private void HighlightLetter()
		{
			_spellName.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_spelledLetterColor)}>{_spellNameCache.Substring(0, _currentLetterIndex + 1)}</color>{_spellNameCache.Substring(_currentLetterIndex + 1)}";
			_currentLetterIndex++;
		}

		/// <summary>
		/// Update spell timer progress bar.
		/// </summary>
		/// <param name="percentage"></param>
		private void UpdateSpellTimer(float percentage) => _spellTimerBar.value = percentage;

		/// <summary>
		/// Reset spell progress text.
		/// </summary>
		private void ResetTextToDefault()
		{
			_currentLetterIndex = 0;
			_spellNameCache = string.Empty;
			/* Decrease font size and character spacing
			 * and reset progress bar text to default text */
			_spellName.fontSize = 30;
			_spellName.characterSpacing = 0;
			_spellName.text = _defaultText;

		}
		#endregion
	}
}