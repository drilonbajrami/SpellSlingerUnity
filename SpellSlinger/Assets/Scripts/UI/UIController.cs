using System;
using UnityEngine;

namespace SpellSlinger
{
    public class UIController : MonoBehaviour
    {
		//[SerializeField] private SpellProgress _spellProgress;
		//[SerializeField] private SpellHint _spellHint;

		//private void Start()
		//{
		//	SpellCrafter.StartCrafting += EnableSpellProgress;		
		//	SpellCrafter.CraftSpell += DisableSpellProgress;
		//	SpellCrafter.LetterSent += OnLetterSpell;
		//	HelpGesture.PoseForm += OpenSpellHint;

		//	_spellHint.gameObject.SetActive(true);
		//	_spellHint.gameObject.SetActive(false);
		//}

		//private void Update()
		//{
		//	if (_spellProgress.gameObject.activeSelf)
		//	{
		//		_spellProgress.UpdateSpellTimer(SpellCrafter._craftingTimer.GetProgressPercentage());
		//	}
		//}

		//private void OpenSpellHint(object source, bool condition) => _spellHint.gameObject.SetActive(condition);

		//private void EnableSpellProgress(object source, SpellType spellType)
		//{
		//	if (!_spellProgress.gameObject.activeSelf)
		//	{
		//		_spellProgress.gameObject.SetActive(true);
		//		Player.Instance.GestureCaster.Enable<HelpGesture>();
		//		_spellHint.ResetPanel();
		//		Player.Instance.GestureCaster.Enable<SwipeGesture>();
		//	}
		//	else if (spellType == null)
		//	{
		//		_spellProgress.ResetText();
		//		_spellHint.ResetPanel();
		//		Player.Instance.GestureCaster.Enable<SwipeGesture>();
		//	}
		//	else // is spellType != null
		//	{
		//		Player.Instance.GestureCaster.Disable<SwipeGesture>();
		//		_spellProgress.SpellToCraft(spellType);
		//		_spellHint.SetCurrentSpellElement(spellType.Element);
		//	}
		//}

		//private void DisableSpellProgress(object source, SpellType spellType)
		//{
		//	Player.Instance.GestureCaster.Disable<HelpGesture>();
		//	Player.Instance.GestureCaster.Disable<SwipeGesture>();
		//	_spellProgress.gameObject.SetActive(false);
		//	_spellHint.ResetPanel();
		//	_spellHint.gameObject.SetActive(false);
		//}

		//private void OnLetterSpell(object source, EventArgs e)
		//{
		//	_spellProgress.HighlightLetter();
		//	_spellHint.NextSignLetter();
		//}
	}
}