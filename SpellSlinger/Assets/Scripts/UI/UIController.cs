using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		[SerializeField] private SpellProgress SpellProgress;
		[SerializeField] private HandHelp HandHelp;

		private void NextLetterSpelled(object source, EventArgs e) => SpellProgress.HighlightNextLetter();

		private void Start()
		{
			GestureReceiver.SpellToCraftEvent += EnableSpellProgressPanel;
			GestureReceiver.NextLetterSpelledEvent += NextLetterSpelled;
			GestureReceiver.CraftedSpellEvent += DisableSpellProgressPanel;

			HelpGesture.PoseEvent += HelpPose;
		}

		private void Update()
		{
			if (SpellProgress.gameObject.activeSelf)
			{
				SpellProgress.UpdateSpellTimer(GestureReceiver._craftingDurationTimer.GetProgressPercentage());
			}
		}

		private void HelpPose(object source, bool open) => HandHelp.gameObject.SetActive(open);

		private void EnableSpellProgressPanel(object source, SpellType spellType)
		{
			if (!SpellProgress.gameObject.activeSelf)
				SpellProgress.gameObject.SetActive(true);

			if(spellType != null)
				SpellProgress.SpellToCraft(spellType);
		}

		private void DisableSpellProgressPanel(object source, SpellType spellType)
		{
			if (SpellProgress.gameObject.activeSelf)
			{
				SpellProgress.ResetText();
				SpellProgress.gameObject.SetActive(false);
			}
		}
	}
}