using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureReceiver : MonoBehaviour
	{
		[Header("Available Spells")]
		[SerializeField] private List<SpellType> _spells;

		private Timer _spellingDurationTimer;
		[Range(1.0f, 10.0f)] [SerializeField] private float _spellingDurationInSeconds = 8.0f;

		private SpellType _currentSpelling = null;
		private int _currentSpellingLetterIndex = 1;
		private bool _spellAvailable = false;

		private bool _readyToCast = false;

		#region UNITY Methods

		private void Start()
		{
			_spellingDurationTimer = new Timer(_spellingDurationInSeconds);
			_spellingDurationTimer.Deactivate();
			_spellingDurationTimer.TimerEnd += OnSpellingTimerEnd;

			GestureCaster.LHandPose += OnLeftHandPoseReceived;
			GestureCaster.RHandPose += OnRightHandPoseReceived;
			GestureCaster.StartSpellingPose += OnStartCastPose;
			GestureCaster.CastSpellPose += OnCastSpellPose;
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

		#region OnEvent Methods

		private void OnLeftHandPoseReceived(object source, string value)
		{
			if (!_spellAvailable)
			{
				_spellAvailable = IsSpellAvailable(char.Parse(value));
				if (_spellAvailable)
					Debug.Log($"Spell is available and its the {_currentSpelling.GetElementTypeName()} spell...");
			}
			else
				CheckNextLetter(char.Parse(value));
		}

		private void OnRightHandPoseReceived(object source, string value)
		{

		}

		private void OnStartCastPose(object source, EventArgs e)
		{
			ResetSpelling();
			_spellingDurationTimer.Activate();
			Debug.Log("Starting to spell...!");
		}

		private void OnCastSpellPose(object source, EventArgs e)
		{
			if (_readyToCast)
				OnCastingSpellFinished();
			else
				OnCastingSpellFailed();
		}

		private void OnSpellingTimerEnd(object source, EventArgs e)
		{
			OnSpellingFailed();
		}

		#endregion

		private void OnSpellingFinished()
		{
			_spellingDurationTimer.Deactivate();
			_readyToCast = true;
			Debug.Log("Spelling Successfully! Now try to cast your spell...");
		}

		private void OnSpellingFailed()
		{
			ResetSpelling();
			Debug.Log("Spelling failed! Try again...");
		}

		private void OnCastingSpellFinished()
		{
			Debug.Log("Spell Casted Successfully!");
			ResetSpelling();
		}

		private void OnCastingSpellFailed()
		{
			Debug.Log("Casting spell failed, there is no spell!");
		}

		private void ResetSpelling()
		{
			_currentSpelling = null;
			_currentSpellingLetterIndex = 1;
			_spellAvailable = false;
			_readyToCast = false;
			_spellingDurationTimer.ResetTimer();
		}

		/// <summary>
		/// Check if there is any spells that start with the given letter
		/// </summary>
		/// <param name="spellFirstLetter"></param>
		/// <returns></returns>
		private bool IsSpellAvailable(char spellFirstLetter)
		{
			_currentSpelling = _spells.Find(a => a.GetElementLetterByIndex(0) == spellFirstLetter);

			if (_currentSpelling != null)
			{
				_spellingDurationTimer.Activate();
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Check if next letter is present on the Spell's name
		/// </summary>
		/// <param name="nextLetter"></param>
		private void CheckNextLetter(char nextLetter)
		{
			if (nextLetter == _currentSpelling.GetElementLetterByIndex(_currentSpellingLetterIndex))
			{
				Debug.Log($"Spell next letter is { nextLetter }");
				_currentSpellingLetterIndex++;

				if (_currentSpellingLetterIndex == _currentSpelling.GetElementTypeNameLength())
					OnSpellingFinished();
			}
		}
	}
}
