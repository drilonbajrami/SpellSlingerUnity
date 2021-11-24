using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class SpellCrafter : MonoBehaviour
	{
		[Header("Available Spells")]
		[SerializeField] private List<SpellType> _spells;

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

		/// <summary>
		/// Timer for a spell to be crafted completely.
		/// </summary>
		public static Timer CraftingTimer;

		#region Events
		public static EventHandler<SpellType> StartCrafting;
		public static EventHandler<SpellType> CraftSpell;
		public static EventHandler LetterSent;

		// Event Rasier Methods
		private void OnStartCrafting(SpellType spellType) => StartCrafting?.Invoke(this, spellType);
		private void OnCraftSpell(SpellType spellType) => CraftSpell?.Invoke(this, spellType);
		private void OnLetterSent() => LetterSent?.Invoke(this, EventArgs.Empty);
		#endregion

		#region UNITY Methods
		private void Start()
		{
			// Setup crafting timer
			CraftingTimer = new Timer(_craftingDuration);
			CraftingTimer.TimerEnd += CraftFailed;

			// Subscribe to gesture events
			CraftGesture.PoseForm += Craft;
			LetterGesture.PoseForm += GetLetter;
		}

		private void Update()
		{
			CraftingTimer.UpdateTimer(Time.deltaTime);
		}

		private void OnValidate()
		{
			if (CraftingTimer != null)
				CraftingTimer.ChangeInterval(_craftingDuration);
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
			Player.Instance.Gestures.Enable<LetterGesture>();
			ResetCrafting();
			OnStartCrafting(_spellType);
		}

		/// <summary>
		/// Called on failing to finish crafting a spell on time.
		/// </summary>
		private void CraftFailed(object source, EventArgs e)
		{
			Player.Instance.Gestures.Disable<LetterGesture>();
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
			CraftingTimer.Stop();
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
				CraftingTimer.Start();
				OnStartCrafting(_spellType);	
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
					Player.Instance.Gestures.Disable<LetterGesture>();
					OnCraftSpell(_spellType);		
					ResetCrafting();
				}
			}
		}
		#endregion
	}
}