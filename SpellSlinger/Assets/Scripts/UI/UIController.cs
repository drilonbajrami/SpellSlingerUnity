using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		[SerializeField] private SpellProgress SpellProgress;
		[SerializeField] private HandHelp HandHelp;

		private SpellType currentSpellType = null;

		

		private void Start()
		{
			GestureReceiver.SpellToCraftEvent += EnableSpellProgress;		
			GestureReceiver.CraftedSpellEvent += DisableSpellProgress;
			GestureReceiver.NextLetterSpelledEvent += NextLetterSpelled;

			HelpGesture.PoseEvent += OpenHandHelp;
		}

		private void Update()
		{
			if (SpellProgress.gameObject.activeSelf)
			{
				SpellProgress.UpdateSpellTimer(GestureReceiver._craftingDurationTimer.GetProgressPercentage());
			}
		}

		private void OpenHandHelp(object source, bool open) => HandHelp.gameObject.SetActive(open);

		private void EnableSpellProgress(object source, SpellType spellType)
		{
			if (!SpellProgress.gameObject.activeSelf)
				SpellProgress.gameObject.SetActive(true);

			if(spellType != null)
				SpellProgress.SpellToCraft(spellType);
		}

		private void DisableSpellProgress(object source, SpellType spellType)
		{
			if (SpellProgress.gameObject.activeSelf)
				SpellProgress.gameObject.SetActive(false);

			HandHelp.RestartPanel();
		}

		private void NextLetterSpelled(object source, EventArgs e) => SpellProgress.HighlightNextLetter();
	}
}