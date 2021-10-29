using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		[SerializeField] private SpellProgress SpellProgressPanel;

		private void SpellToCraft(object source, SpellType spellType) => EnableSpellProgressPanel(spellType);
		private void NextLetterSpelled(object source, EventArgs e) => SpellProgressPanel.HighlightNextLetter();
		private void CraftSpell(object source, SpellType spellType) => DisableSpellProgressPanel();
		private void CraftFailed(object source, EventArgs e) => DisableSpellProgressPanel();

		private void Start()
		{
			GestureReceiver.SpellToCraft += SpellToCraft;
			GestureReceiver.NextLetterSpelled += NextLetterSpelled;
			GestureReceiver.CraftSpell += CraftSpell;
			GestureReceiver.CraftFailed += CraftFailed;
		}

		private void Update()
		{
			if (SpellProgressPanel.gameObject.activeSelf)
			{
				SpellProgressPanel.UpdateSpellTimer(GestureReceiver._spellingDurationTimer.GetProgressPercentage());
			}
		}

		private void EnableSpellProgressPanel(SpellType spellType)
		{
			if (!SpellProgressPanel.gameObject.activeSelf)
			{
				SpellProgressPanel.gameObject.SetActive(true);
				SpellProgressPanel.SpellToCraft(spellType);
			}
		}

		private void DisableSpellProgressPanel(SpellType spellType = null)
		{
			if (SpellProgressPanel.gameObject.activeSelf)
			{
				SpellProgressPanel.ResetText();
				SpellProgressPanel.gameObject.SetActive(false);
			}
		}
	}
}