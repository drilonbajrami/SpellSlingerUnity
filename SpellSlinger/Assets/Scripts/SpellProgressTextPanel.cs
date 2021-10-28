using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpellSlinger
{
	public class SpellProgressTextPanel : MonoBehaviour
	{
		private TMP_Text _spellName;
		private string _spellNameText;
		private int _currentLetterIndex = 0;

		private void OnNextLetterSpelled(object source, EventArgs e) => HighlightNextLetter();
		private void OnSetSpellName(object source, string spellName) => SetSpellName(spellName);
		private void OnCreateSpell(object source, SpellType spellType) => CreateSpell();

		private void Start()
		{
			_spellName = transform.GetChild(0).GetComponent<TMP_Text>();
			GestureReceiver.NextLetterSpelled += OnNextLetterSpelled;
			GestureReceiver.SpellToBeSpelled += OnSetSpellName;
			GestureReceiver.CreateSpell += OnCreateSpell;
		}

		private void SetSpellName(string spellName)
		{
			_currentLetterIndex = 0;
			_spellName.text = spellName;
			_spellNameText = spellName;
			HighlightNextLetter();
		}

		private void HighlightNextLetter()
		{
			_spellName.text = $"<color=red>{_spellNameText.Substring(0, _currentLetterIndex + 1)}</color>{_spellNameText.Substring(_currentLetterIndex + 1)}";
			_currentLetterIndex++;
		}

		private void CreateSpell()
		{
			_spellName.text = string.Empty;
			_spellNameText = string.Empty;
		}
	}
}
