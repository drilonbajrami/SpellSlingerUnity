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
            if (tutorial.IsInStep(StepIndex.CRAFT))
            {
                _player.Gestures.Disable<LetterGesture>();
                _player.Gestures.Disable<ThumbsUpGesture>();
                tutorial.NextStep();
            }
            else
            {
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
            if(tutorial.IsInStep(StepIndex.HELP))
                tutorial.NextStep();
        }

        // SWIPE GESTURE
        private void OnSwipeGesture(object sender, EventArgs e)
        {
            if (tutorial.IsInStep(StepIndex.SWIPE))
            {
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
                tutorial.NextStep();
                SpellCrafter.StartCrafting -= OnStartCrafting;
                SpellCrafter.CraftSpell += OnCraftSpell;
                SpellCrafter.CraftingTimer.Stop();
            }    
        }

        private void OnCraftSpell(object sender, SpellType e)
        {
            _player.Gestures.Enable<CastGesture>();
            SpellCrafter.CraftSpell -= OnCraftSpell;
            tutorial.JumpToStep(StepIndex.CAST);
        }

        // CAST GESTURE
        private void OnCastGesture(object sender, EventArgs e)
        {
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
            if(tutorial.IsInStep(StepIndex.LAST))
                _player.ChangeState(new PlayState(_player));
        }
        #endregion

        #region Inherited Methods
        public TutorialState(Player player) : base(player) { }

        public override void OnEnter()
        {
            // Cache reference to tutorial
            tutorial = GameManager.Instance.Tutorial;
            tutorial.Activate(true);

            // Subscribe to gestures
            CraftGesture.PoseForm += OnCraftGesture;
            CastGesture.PoseForm += OnCastGesture;
            HelpGesture.PoseForm += OnHelpGesture;
            SwipeGesture.PoseForm += OnSwipeGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;

            // For first step
            _player.Gestures.Enable<CraftGesture>();     
        }

        public override void OnExit()
        {
            tutorial.Activate(false);
            _player.Gestures.Disable<ThumbsUpGesture>();
        }
        #endregion
    }
}