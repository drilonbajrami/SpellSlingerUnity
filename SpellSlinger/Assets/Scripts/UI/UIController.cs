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

			HelpGesture.PoseEvent += OpenHandHelp;
		}

		private void Update()
		{
			if (spellProgress.gameObject.activeSelf)
			{
				spellProgress.UpdateSpellTimer(SpellCrafter._craftingTimer.GetProgressPercentage());
			}
		}

		private void OpenHandHelp(object source, bool condition) => spellHint.gameObject.SetActive(condition);

		private void EnableSpellProgress(object source, SpellType spellType)
		{
			if (!spellProgress.gameObject.activeSelf)
				spellProgress.gameObject.SetActive(true);

			if (spellType != null)
			{
				spellProgress.SpellToCraft(spellType);
				spellHint.SetCurrentSpellElement(spellType);
			}
		}

		private void DisableSpellProgress(object source, SpellType spellType)
		{
			if (spellProgress.gameObject.activeSelf)
				spellProgress.gameObject.SetActive(false);

			spellHint.ResetPanel();
		}

		private void OnLetterSpell(object source, EventArgs e)
		{
			spellProgress.HighlightLetter();
			spellHint.Refresh();
		}
	}
}