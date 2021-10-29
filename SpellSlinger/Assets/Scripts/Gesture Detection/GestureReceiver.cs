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
		/// Timer for a spell to be completed.
		/// </summary>
		public static Timer _spellingDurationTimer;
		[Range(1.0f, 10.0f)] [SerializeField] private float _spellingDurationInSeconds = 8.0f;

		/// <summary>
		/// Keep track the type of the current spell being spelled.
		/// </summary>
		private SpellType _currentSpelling = null;

		/// <summary>
		/// Keep track of which letter is to be spelled next.
		/// </summary>
		private int _currentSpellingLetterIndex = 1;

		private bool _isSpellAvailable = false;
		
		/// <summary>
		/// Checks if we are ready to cast a spell, true only if we crafted a spell beforehand.
		/// </summary>
		private bool _isReadyToCast = false;

		#endregion

		#region Event Handlers & Raisers

		/// <summary>
		/// Event Handler for showing the spell type to be crafted.
		/// </summary>
		public static event EventHandler<SpellType> SpellToCraft;
		public virtual void OnSpellToCraft() => SpellToCraft?.Invoke(this, _currentSpelling);

		/// <summary>
		/// Event Handler for next letter spelled correctly.
		/// </summary>
		public static event EventHandler NextLetterSpelled;
		public virtual void OnNextLetterSpelled() => NextLetterSpelled?.Invoke(this, EventArgs.Empty);

		/// <summary>
		/// Event Handler for finished crafting of a spell.
		/// </summary>
		public static event EventHandler<SpellType> CraftSpell;
		public virtual void OnCraftSpell() => CraftSpell?.Invoke(this, _currentSpelling);

		/// <summary>
		/// Event Handler for casting a spell.
		/// </summary>
		public static event EventHandler CastSpell;
		public virtual void OnCastSpell() => CastSpell?.Invoke(this, EventArgs.Empty);

		/// <summary>
		/// Event Handler for when crafting a spell fails due to time out.
		/// </summary>
		public static event EventHandler CraftFailed;
		public virtual void OnCraftFailed() => CraftFailed?.Invoke(this, EventArgs.Empty);

		#endregion

		#region UNITY Methods

		private void Start()
		{
			_spellingDurationTimer = new Timer(_spellingDurationInSeconds);
			_spellingDurationTimer.Deactivate();
			_spellingDurationTimer.TimerFinish += OnSpellingTimerEnd;

			GestureCaster.LeftHandPose += LeftHandPose;
			GestureCaster.RightHandPose += RightHandPose;
			GestureCaster.CraftPose += CraftPose;
			GestureCaster.CastPose += CastPose;
		}

		private void Update()
		{
			_spellingDurationTimer.UpdateTimer(Time.deltaTime);
		}

		private void OnValidate()
		{
			if (_spellingDurationTimer != null)
				_spellingDurationTimer.ChangeInterval(_spellingDurationInSeconds);
		}

		#endregion

		#region Event Subscriber Methods

		/// <summary>
		/// Called on receiving left hand poses.
		/// </summary>
		private void LeftHandPose(object source, string value)
		{
			// Check if there is an available spell type to be crafted
			if (!_isSpellAvailable)
				_isSpellAvailable = IsSpellAvailable(char.Parse(value));
			else
				CheckNextLetterSpelled(char.Parse(value));
		}

		/// <summary>
		/// Called on receiving right hand poses.
		/// </summary>
		private void RightHandPose(object source, string value)
		{

		}

		/// <summary>
		/// Called on receiving craft pose.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void CraftPose(object source, EventArgs e)
		{
			ResetSpellCrafting();
			_spellingDurationTimer.Activate();
		}

		/// <summary>
		/// Called on receiving cast pose.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void CastPose(object source, EventArgs e)
		{
			if (_isReadyToCast) CastingSpellSucceded();
			else CastingSpellFailed();
		}

		/// <summary>
		/// Called on failing to finish crafting a spell on time.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void OnSpellingTimerEnd(object source, EventArgs e) => CraftingSpellFailed();

		#endregion

		#region Private Methods

		/// <summary>
		/// Call this when crafting a spell is successful
		/// </summary>
		private void CraftingSpellSucceded()
		{
			OnCraftSpell();
			_spellingDurationTimer.Deactivate();
			_isReadyToCast = true;
		}

		/// <summary>
		/// Call this when crafting a spell failed.
		/// </summary>
		private void CraftingSpellFailed()
		{
			OnCraftFailed();
			ResetSpellCrafting(); 
		}

		/// <summary>
		/// Call this when casting a spell is successful.
		/// </summary>
		private void CastingSpellSucceded()
		{
			OnCastSpell();
			ResetSpellCrafting();
		}

		/// <summary>
		/// Call this when casting a spell failed.
		/// </summary>
		private void CastingSpellFailed()
		{

		}

		/// <summary>
		/// Resets everything related to crafting a spell.
		/// </summary>
		private void ResetSpellCrafting()
		{
			_currentSpelling = null;
			_currentSpellingLetterIndex = 1;
			_isSpellAvailable = false;
			_isReadyToCast = false;
			_spellingDurationTimer.ResetTimer();
		}

		/// <summary>
		/// Check if there is any spells that start with the given letter.
		/// Invokes the SpellToCraft event.
		/// </summary>
		private bool IsSpellAvailable(char spellFirstLetter)
		{
			_currentSpelling = _spells.Find(a => a.GetElementLetterByIndex(0) == spellFirstLetter);

			// Cache the spell type if it is available
			if (_currentSpelling != null)
			{
				_spellingDurationTimer.Activate();
				OnSpellToCraft();
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Check if next letter spelled is present on the spell's name.
		/// Depending on the progress of spell crafting, it raises the NextLetterSpelled event,
		/// or the SpellingFinished event if all letters have been spelled correctly.
		/// </summary>
		private void CheckNextLetterSpelled(char nextLetter)
		{
			if (nextLetter == _currentSpelling.GetElementLetterByIndex(_currentSpellingLetterIndex))
			{
				OnNextLetterSpelled();
				_currentSpellingLetterIndex++;

				// Check if all letters have been spelled already
				if (_currentSpellingLetterIndex == _currentSpelling.GetElementTypeNameLength())
					CraftingSpellSucceded();
			}
		}

		#endregion
	}
}