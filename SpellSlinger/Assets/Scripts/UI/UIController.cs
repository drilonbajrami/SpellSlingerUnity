using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		[SerializeField] private SpellProgress SpellProgressPanel;

		private void SpellToCraft(object source, SpellType spellType) => EnableSpellProgressPanel(spellType);
		private void NextLetterSpelled(object source, EventArgs e) => SpellProgressPanel.HighlightNextLetter();
		private void CraftedSpell(object source, SpellType spellType) => DisableSpellProgressPanel(spellType);

		private void Start()
		{
			GestureReceiver.SpellToCraftEvent += SpellToCraft;
			GestureReceiver.NextLetterSpelledEvent += NextLetterSpelled;
			GestureReceiver.CraftedSpellEvent += CraftedSpell;
		}

		private void Update()
		{
			if (SpellProgressPanel.gameObject.activeSelf)
			{
				SpellProgressPanel.UpdateSpellTimer(GestureReceiver._craftingDurationTimer.GetProgressPercentage());
			}
		}

		private void EnableSpellProgressPanel(SpellType spellType)
		{
			if (!SpellProgressPanel.gameObject.activeSelf)
				SpellProgressPanel.gameObject.SetActive(true);

			if(spellType != null)
				SpellProgressPanel.SpellToCraft(spellType);
		}

		private void DisableSpellProgressPanel(SpellType spellType)
		{
			if (SpellProgressPanel.gameObject.activeSelf)
			{
				SpellProgressPanel.ResetText();
				SpellProgressPanel.gameObject.SetActive(false);
			}
		}
	}
}