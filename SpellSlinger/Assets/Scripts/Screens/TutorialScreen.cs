using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TutorialScreen : MonoBehaviour
    {
        [SerializeField] private Tutorial Tutorial;

        #region UNITY
        private void OnEnable()
        {
            Player.Instance.Gestures.DisableAllGestures();
            CraftGesture.PoseForm += OnCraftGesture;
            CastGesture.PoseForm += OnCastGesture;
            HelpGesture.PoseForm += OnHelpGesture;
            SwipeGesture.PoseForm += OnSwipeGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;
            Player.Instance.Gestures.Enable<CraftGesture>();
        }

        private void OnDisable()
        {
            CraftGesture.PoseForm -= OnCraftGesture;
            CastGesture.PoseForm -= OnCastGesture;
            HelpGesture.PoseForm -= OnHelpGesture;
            SwipeGesture.PoseForm -= OnSwipeGesture;
            ThumbsUpGesture.PoseForm -= OnThumbsUp; 
        }
        #endregion

        #region Event Subscriber Methods
        // CRAFT GESTURE
        private void OnCraftGesture(object sender, EventArgs e)
        {
            // If we still in the first step then disable
            // the letter gesture manually since it always
            // gets enabled when doing the craft gesture within
            // the SpellCrafter class
            if (Tutorial.IsInStep(StepIndex.CRAFT))
            {
                Player.Instance.Gestures.Disable<LetterGesture>();
                Tutorial.NextStep(); // To step (2 - HELP)
            }
            else
            {
                // In whatever step we are, restart and go to the second step
                // since we already went through the first step once (1 - CRAFT)
                Player.Instance.Gestures.Disable<LetterGesture>();
                Player.Instance.Gestures.Disable<CastGesture>();
                Player.Instance.Gestures.Disable<ThumbsUpGesture>();
                Tutorial.JumpToStep(StepIndex.HELP);
                SpellCrafter.StartCrafting -= OnStartCrafting;
            }
        }

        // HELP GESTURE
        private void OnHelpGesture(object sender, bool e)
        {
            // We go to the next step (3 - SWIPE)
            if (Tutorial.IsInStep(StepIndex.HELP))
                Tutorial.NextStep();
        }

        // SWIPE GESTURE
        private void OnSwipeGesture(object sender, EventArgs e)
        {
            if (Tutorial.IsInStep(StepIndex.SWIPE))
            {
                // Since we go to the next step (4 -  SELECT)
                // we subscribe to SpellCrafter's event for carft starting
                // and enable LetterGesture since we need to listen to spelled letters
                Tutorial.NextStep();
                SpellCrafter.StartCrafting += OnStartCrafting;
                Player.Instance.Gestures.Enable<LetterGesture>();
            }
        }

        // LETTER GESTURE & CRAFTING
        private void OnStartCrafting(object sender, SpellType e)
        {
            if (e != null)
            {
                // If we selected a spell type to craft, then
                // disable crafting timer for tutorial purposes
                // and subscribe for the SpellCrafter's CraftSpell event
                // in order to know when we complete crafting a spell
                Tutorial.NextStep();
                SpellCrafter.StartCrafting -= OnStartCrafting;
                SpellCrafter.CraftSpell += OnCraftSpell;
                SpellCrafter.CraftingTimer.Pause();
            }
        }

        private void OnCraftSpell(object sender, SpellType e)
        {
            // When a spell is completely crafted, we enable the 
            // Casting Gesture, so we can be able to throw the spell
            Player.Instance.Gestures.Enable<CastGesture>();
            SpellCrafter.CraftSpell -= OnCraftSpell;
            Tutorial.JumpToStep(StepIndex.CAST);
        }

        // CAST GESTURE
        private void OnCastGesture(object sender, EventArgs e)
        {
            // Cast(throw) the spell and continue to next step
            if (Tutorial.IsInStep(StepIndex.CAST))
            {
                Tutorial.NextStep();
                Player.Instance.Gestures.Disable<CastGesture>();
                Player.Instance.Gestures.Enable<ThumbsUpGesture>();
                SpellCrafter.CraftingTimer.Continue();
            }
        }

        // THUMBS UP GESTURE
        private void OnThumbsUp(object sender, EventArgs e)
        {
            if (Tutorial.IsInStep(StepIndex.LAST))
            {
                gameObject.SetActive(false);
                GameManager.Instance.SettingsSreen.SetActive(true);
                GameManager.Instance.SpellCrafter.Toggle(false);
            }
        }
        #endregion
    }
}
