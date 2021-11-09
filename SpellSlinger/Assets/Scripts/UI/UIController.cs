using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		[SerializeField] private SpellProgress spellProgress;
		[SerializeField] private SpellHint spellHint;

		private void Start()
		{
			SpellCrafter.StartCrafting += EnableSpellProgress;		
			SpellCrafter.CraftSpell += DisableSpellProgress;
			SpellCrafter.LetterSent += OnLetterSpell;
		}

		private void Update()
		{
			if (spellProgress.gameObject.activeSelf)
			{
				spellProgress.UpdateSpellTimer(SpellCrafter._craftingTimer.GetProgressPercentage());
			}
		}

		private void OpenSpellHint(object source, bool condition) => spellHint.gameObject.SetActive(condition);

		private void EnableSpellProgress(object source, SpellType spellType)
		{
			if (!spellProgress.gameObject.activeSelf)
			{
				spellProgress.gameObject.SetActive(true);
				HelpGesture.PoseForm += OpenSpellHint;
			}

			if (spellType != null)
			{
				spellProgress.SpellToCraft(spellType);
				spellHint.SetCurrentSpellElement(spellType.Properties.GetElementType());
			}
		}

		private void DisableSpellProgress(object source, SpellType spellType)
		{
			if (spellProgress.gameObject.activeSelf)
			{
				spellProgress.gameObject.SetActive(false);
				HelpGesture.PoseForm -= OpenSpellHint;
			}

			spellHint.ResetPanel();
			spellHint.gameObject.SetActive(false);
		}

		private void OnLetterSpell(object source, EventArgs e)
		{
			spellProgress.HighlightLetter();
			spellHint.NextSignLetter();
		}
	}
}