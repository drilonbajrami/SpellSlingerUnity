using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureReceiver : MonoBehaviour
	{
		#region Private Fields
		[Header("Available Spells")]
		[SerializeField] private List<SpellType> _spells;

		/// <summary>
		/// Timer for a spell to be crafted completely.
		/// </summary>
		public static Timer _craftingDurationTimer;
		[Range(1.0f, 15.0f)] [SerializeField] private float _craftingDurationInSeconds = 8.0f;

		/// <summary>
		/// Keep track the type of the current spell being spelled.
		/// </summary>
		private SpellType _currentSpell = null;

		/// <summary>
		/// Keep track of which letter is to be spelled next.
		/// </summary>
		private int _currentSpellingLetterIndex = 1;

		/// <summary>
		/// Keep track if there is a spell available to be crafted
		/// </summary>
		private bool _isSpellAvailable = false;

		/// <summary>
		/// Keep track if we are currently crafting any spells
		/// </summary>
		private bool _isCurrentlyCrafting = false;
		
		/// <summary>
		/// Checks if we are ready to cast a spell, true only if we crafted a spell beforehand.
		/// </summary>
		private bool _isReadyToCast = false;
		#endregion

		#region Event Handlers
		// For UI
		public static EventHandler<SpellType> SpellToCraftEvent;
		public static EventHandler<SpellType> CraftedSpellEvent;
		public static EventHandler NextLetterSpelledEvent;
		public static EventHandler CastSpellEvent;	
		#endregion

		#region Event Raisers
		private void OnSpellToCraft() => SpellToCraftEvent?.Invoke(this, _currentSpell);
		private void OnCraftedSpell(SpellType spellType) => CraftedSpellEvent?.Invoke(this, spellType);
		private void OnNextLetterSpelled() => NextLetterSpelledEvent?.Invoke(this, EventArgs.Empty);
		private void OnCastSpell() => CastSpellEvent?.Invoke(this, EventArgs.Empty);
		#endregion

		#region UNITY Methods
		private void Start()
		{
			_craftingDurationTimer = new Timer(_craftingDurationInSeconds);
			_craftingDurationTimer.Pause();
			_craftingDurationTimer.TimerEnd += CraftingDurationTimerFinish;

			LetterGesture.PoseEvent += LetterPose;
			CraftGesture.PoseEvent += CraftPose;
			CastGesture.PoseEvent += CastPose;
		}

		private void Update()
		{
			_craftingDurationTimer.UpdateTimer(Time.deltaTime);
		}

		private void OnValidate()
		{
			if (_craftingDurationTimer != null)
				_craftingDurationTimer.ChangeInterval(_craftingDurationInSeconds);
		}
		#endregion

		#region Event Subscriber Methods
		/// <summary>
		/// Called on receiving left hand poses.
		/// </summary>
		private void LetterPose(object source, char letter)
		{
			// Check if player is currently on crafting state
			if (_isCurrentlyCrafting)
			{
				if (!_isSpellAvailable)
				{		
					_isSpellAvailable = IsSpellAvailable(letter);
					if (_isSpellAvailable) _craftingDurationTimer.Continue();
				}
				else
					CheckNextLetterSpelled(letter);
			}
		}

		/// <summary>
		/// Called on receiving craft pose.
		/// </summary>
		private void CraftPose(object source, EventArgs e)
		{
			ResetSpellCrafting();
			_isCurrentlyCrafting = true;
			OnSpellToCraft(); // Raises SpellToCraftEvent
		}

		/// <summary>
		/// Called on receiving cast pose.
		/// </summary>
		private void CastPose(object source, EventArgs e)
		{
			if (_isReadyToCast)
			{
				OnCastSpell();
				ResetSpellCrafting();
			}
		}

		/// <summary>
		/// Called on failing to finish crafting a spell on time.
		/// </summary>
		private void CraftingDurationTimerFinish(object source, EventArgs e)
		{
			OnCraftedSpell(null); // Crafting the spell has failed due to overtime
			ResetSpellCrafting();
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Resets everything related to crafting a spell.
		/// </summary>
		private void ResetSpellCrafting()
		{
			_isCurrentlyCrafting = false;
			_currentSpell = null;
			_currentSpellingLetterIndex = 1;
			_isSpellAvailable = false;
			_isReadyToCast = false;
			_craftingDurationTimer.Start();
		}

		/// <summary>
		/// Check if there is any spells that start with the given letter.
		/// Invokes the SpellToCraft event.
		/// </summary>
		private bool IsSpellAvailable(char spellFirstLetter)
		{
			_currentSpell = _spells.Find(a => a.Properties.GetElementLetterByIndex(0) == spellFirstLetter);

			// Cache the spell type if it is available
			if (_currentSpell != null)
			{
				OnSpellToCraft(); // Raises SpellToCraftEvent
				_craftingDurationTimer.Continue();
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Check if next letter spelled is present on the spell's name.
		/// Depending on the progress of spell crafting, it raises the NextLetterSpelled event,
		/// or the SpellingFinished event if all letters have been spelled correctly.
		/// </summary>
		private void CheckNextLetterSpelled(char nextLetter)
		{
			if (nextLetter == _currentSpell.Properties.GetElementLetterByIndex(_currentSpellingLetterIndex))
			{
				OnNextLetterSpelled(); // Raises NextLetterSpelledEvent
				_currentSpellingLetterIndex++;

				// Check if all letters have been spelled already
				if (_currentSpellingLetterIndex == _currentSpell.Properties.GetElementTypeNameLength())
				{
					OnCraftedSpell(_currentSpell); // Raises CraftSpellEvent
					_craftingDurationTimer.Pause();
					_isReadyToCast = true;
				}
			}
		}
		#endregion
	}
}