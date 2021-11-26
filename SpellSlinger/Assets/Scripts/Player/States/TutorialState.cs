using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TutorialState : State
    {
        private Tutorial tutorial;

        #region Event Subscriber Methods
        // CRAFT GESTURE
        private void OnCraftGesture(object sender, EventArgs e)
        {
            // If we still in the first step then disable
            // the letter gesture manually since it always
            // gets enabled when doing the craft gesture within
            // the SpellCrafter class
            if (tutorial.IsInStep(StepIndex.CRAFT))
            {
                _player.Gestures.Disable<LetterGesture>();
                _player.Gestures.Disable<ThumbsUpGesture>();
                tutorial.NextStep(); // To step (2 - HELP)
            }
            else 
            {
                // In whatever step we are, restart and go to the second step
                // since we already went through the step (1 - CRAFT)
                _player.Gestures.Disable<LetterGesture>();
                _player.Gestures.Disable<CastGesture>();
                _player.Gestures.Disable<ThumbsUpGesture>();
                tutorial.JumpToStep(StepIndex.HELP);
                SpellCrafter.StartCrafting -= OnStartCrafting;
            }
        }

        // HELP GESTURE
        private void OnHelpGesture(object sender, bool e)
        {
            // We go to the next step (3 - SWIPE)
            if (tutorial.IsInStep(StepIndex.HELP))
                tutorial.NextStep();
        }

        // SWIPE GESTURE
        private void OnSwipeGesture(object sender, EventArgs e)
        {
            if (tutorial.IsInStep(StepIndex.SWIPE))
            {
                // Since we go to the next step (4 -  SELECT)
                // we subscribe to SpellCrafter's event for carft starting
                // and enable LetterGesture since we need to listen to spelled letters
                tutorial.NextStep();
                SpellCrafter.StartCrafting += OnStartCrafting;
                _player.Gestures.Enable<LetterGesture>();
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
                tutorial.NextStep();
                SpellCrafter.StartCrafting -= OnStartCrafting;
                SpellCrafter.CraftSpell += OnCraftSpell;
                SpellCrafter.CraftingTimer.Stop();
            }
        }

        private void OnCraftSpell(object sender, SpellType e)
        {
            // When a spell is completely crafted, we enable the 
            // Casting Gesture, so we can be able to throw the spell
            _player.Gestures.Enable<CastGesture>();
            SpellCrafter.CraftSpell -= OnCraftSpell;
            tutorial.JumpToStep(StepIndex.CAST);
        }

        // CAST GESTURE
        private void OnCastGesture(object sender, EventArgs e)
        {
            // Cast(throw) the spell and continue to next step
            if (tutorial.IsInStep(StepIndex.CAST))
            {
                tutorial.NextStep();
                _player.Gestures.Disable<CastGesture>();
                _player.Gestures.Enable<ThumbsUpGesture>();
            }
        }

        // THUMBS UP GESTURE
        private void OnThumbsUp(object sender, EventArgs e)
        {
            //if (tutorial.IsInStep(StepIndex.LAST))
                //_player.ChangeState(new SettingsState(_player));
        }
        #endregion

        #region Inherited Methods
        public TutorialState(Player player) : base(player) { }

        public override void OnEnter()
        {
            // Cache reference to tutorial
            //tutorial = GameManager.Instance.Tutorial;
            //tutorial.Activate(true);

            // Subscribe to all gestures since we need to
            // teach the player how to use each one of them
            CraftGesture.PoseForm += OnCraftGesture;
            CastGesture.PoseForm += OnCastGesture;
            HelpGesture.PoseForm += OnHelpGesture;
            SwipeGesture.PoseForm += OnSwipeGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;

            // For the first step tutorial step
            _player.Gestures.Enable<CraftGesture>();
        }

        public override void OnExit()
        {
            tutorial.Activate(false);

            // Crafting timer set to continue since
            // previously it was not needed for the tutorial
            // part of crafting a spell
            SpellCrafter.CraftingTimer.Continue();

            // Unsubscribe from all gestures
            CraftGesture.PoseForm -= OnCraftGesture;
            CastGesture.PoseForm -= OnCastGesture;
            HelpGesture.PoseForm -= OnHelpGesture;
            SwipeGesture.PoseForm -= OnSwipeGesture;
            ThumbsUpGesture.PoseForm -= OnThumbsUp;

            _player.Gestures.DisableAllGestures();
        }
        #endregion
    }
}