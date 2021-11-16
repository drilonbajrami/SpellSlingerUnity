using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class SpellCrafter : MonoBehaviour
	{
		#region Private Fields
		[Header("Available Spells")]
		[SerializeField] private List<SpellType> _spells;

		/// <summary>
		/// Timer for a spell to be crafted completely.
		/// </summary>
		public static Timer _craftingTimer;
		[Tooltip("Crafting timer duration in seconds.")]
		[Range(1.0f, 15.0f)] [SerializeField] private float _craftingDuration = 8.0f;

		/// <summary>
		/// Keep track the type of the current spell being crafted.
		/// </summary>
		private SpellType _spellType = null;

		/// <summary>
		/// Keep track of which letter is to be spelled next.
		/// </summary>
		private int _currentLetterIndex = 1;
		#endregion

		#region Event Handlers
		public static EventHandler<SpellType> StartCrafting;
		public static EventHandler<SpellType> CraftSpell;
		public static EventHandler LetterSent;
		#endregion

		#region Event Raisers
		private void OnStartCrafting(SpellType spellType) => StartCrafting?.Invoke(this, spellType);
		private void OnCraftSpell(SpellType spellType) => CraftSpell?.Invoke(this, spellType);
		private void OnLetterSent() => LetterSent?.Invoke(this, EventArgs.Empty);
		#endregion

		#region UNITY Methods
		private void Start()
		{
			// Setup crafting timer
			_craftingTimer = new Timer(_craftingDuration);
			_craftingTimer.TimerEnd += CraftFailed;

			// Subscribe to gesture events
			CraftGesture.PoseForm += Craft;
		}

		private void Update()
		{
			_craftingTimer.UpdateTimer(Time.deltaTime);
		}

		private void OnValidate()
		{
			if (_craftingTimer != null)
				_craftingTimer.ChangeInterval(_craftingDuration);
		}
		#endregion

		#region Event Subscriber Methods
		/// <summary>
		/// Called on receiving letter poses.
		/// </summary>
		private void GetLetter(object source, char letter)
		{
			if (_spellType != null) NextLetter(letter);
			else IsSpellAvailable(letter);
		}

		/// <summary>
		/// Called on receiving craft pose.
		/// </summary>
		private void Craft(object source, EventArgs e)
		{
			ResetCrafting();
			OnStartCrafting(_spellType);
			LetterGesture.PoseForm += GetLetter;
		}

		/// <summary>
		/// Called on failing to finish crafting a spell on time.
		/// </summary>
		private void CraftFailed(object source, EventArgs e)
		{
			// Pass spell type as null since crafting failed due to timer ending
			ResetCrafting();
			OnCraftSpell(null);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Resets everything related to crafting a spell.
		/// </summary>
		private void ResetCrafting()
		{
			LetterGesture.PoseForm -= GetLetter;
			_craftingTimer.Stop();
			_currentLetterIndex = 1;
			_spellType = null;
		}

		/// <summary>
		/// Check if there is any spells that start with the given letter.
		/// Invokes the SpellToCraft event.
		/// </summary>
		private void IsSpellAvailable(char spellFirstLetter)
		{
			// Cache the spell type if it is available
			_spellType = _spells.Find(a => a.GetElementLetterByIndex(0) == spellFirstLetter);

			// If spell is available
			if (_spellType != null)
			{
				OnStartCrafting(_spellType);
				_craftingTimer.Start();
			}
		}

		/// <summary>
		/// Check if current spelled letter is present on the spell's name.
		/// Depending on the progress of spell crafting, it raises the LetterSpelled event,
		/// or the CraftingEnd event if all letters have been spelled correctly.
		/// </summary>
		private void NextLetter(char nextLetter)
		{
			if (nextLetter == _spellType.GetElementLetterByIndex(_currentLetterIndex))
			{
				OnLetterSent();
				_currentLetterIndex++;

				// Check if all letters have been spelled already
				if (_currentLetterIndex == _spellType.GetElementTypeNameLength())
				{
					OnCraftSpell(_spellType);
					ResetCrafting();
				}
			}
		}
		#endregion
	}
}