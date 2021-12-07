using System;
using UnityEngine;

namespace SpellSlinger
{
    public class TutorialScreen : MonoBehaviour
    {
        [SerializeField] private Tutorial Tutorial;

        private void OnEnable()
        {
            // Subscribe to all gestures since in the tutorial screen
            // we need to use all of them at least once
            CraftGesture.PoseDetected += OnCraftGesture;
                CastGesture.PoseDetected += OnCastGesture;
                HelpGesture.PoseDetected += OnHelpGesture;
               SwipeGesture.PoseDetected += OnSwipeGesture;
            ThumbsUpGesture.PoseDetected += OnThumbsUp;   
        }

        private void OnDisable()
        {
            // Unsubcribe from all possible gestures on exit
               CraftGesture.PoseDetected -= OnCraftGesture;
                CastGesture.PoseDetected -= OnCastGesture;
                HelpGesture.PoseDetected -= OnHelpGesture;
               SwipeGesture.PoseDetected -= OnSwipeGesture;
            ThumbsUpGesture.PoseDetected -= OnThumbsUp; 
        }

        #region Event Subscriber Methods
        // Step 1 - CRAFT GESTURE
        private void OnCraftGesture(object sender, EventArgs e)
        {
            // If we still in the first step then disable
            // the letter gesture manually since it always
            // gets enabled when doing the craft gesture within
            // the SpellCrafter class
            if (Tutorial.IsInStep(StepIndex.CRAFT)) {
                Player.Instance.Gestures.Disable<LetterGesture>();
                Tutorial.NextStep(); // ---> Step 2 (Help Gesture/Panel)
            }
            else { // Restart and go to the second step since we already did the first step once (1 - CRAFT)
                Player.Instance.Gestures.Disable<LetterGesture>();
                Player.Instance.Gestures.Disable<CastGesture>();
                Player.Instance.Gestures.Disable<ThumbsUpGesture>();
                Tutorial.JumpToStep(StepIndex.HELP);

                // When craft pose is received, we do not want to listen to
                // the SpellCrafter startCrafting event since we already did start
                SpellCrafter.StartCrafting -= OnStartCrafting;
            }
        }

        // Step 2 - HELP GESTURE
        private void OnHelpGesture(object sender, bool e) {      
            if (Tutorial.IsInStep(StepIndex.HELP)) Tutorial.NextStep(); // We go to the next step (3 - SWIPE)
        }

        // Step 3 - SWIPE GESTURE
        private void OnSwipeGesture(object sender, EventArgs e) {
            if (Tutorial.IsInStep(StepIndex.SWIPE)) {
                // Since we go to the next step (4 -  SELECT)
                // we subscribe to SpellCrafter's event for craft starting
                // and enable LetterGesture since we need to listen to spelled letters
                Tutorial.NextStep();
                SpellCrafter.StartCrafting += OnStartCrafting;
                Player.Instance.Gestures.Enable<LetterGesture>();
            }
        }

        // Step 4 - LETTER GESTURE & CRAFTING
        private void OnStartCrafting(object sender, SpellType e) {
            if (e != null) {
                // If we selected a spell type to craft, then disable crafting timer
                // for tutorial purposes and subscribe for the SpellCrafter's CraftSpell event
                // in order to know when we complete crafting a spell
                Tutorial.NextStep();
                SpellCrafter.StartCrafting -= OnStartCrafting;
                SpellCrafter.CraftSpell += OnCraftSpell;
                SpellCrafter.CraftingTimer.Pause();
            }
        }

        // Step 4 - Waiting to craft spell and continue to step (5 - CAST)
        private void OnCraftSpell(object sender, SpellType e) {
            // When a spell is completely crafted, we enable the 
            // Casting Gesture, so we can be able to throw the spell
            Player.Instance.Gestures.Enable<CastGesture>();
            SpellCrafter.CraftSpell -= OnCraftSpell;
            Tutorial.JumpToStep(StepIndex.CAST);
        }

        // Step 5 - CAST GESTURE
        private void OnCastGesture(object sender, EventArgs e) {
            if (Tutorial.IsInStep(StepIndex.CAST)) { // Cast(throw) the spell and continue to next step
                Tutorial.NextStep();
 
                // Enable Cast & ThumbsUp Gestures, we can choose to either start the game or craft again
                Player.Instance.Gestures.Disable<CastGesture>();
                Player.Instance.Gestures.Enable<ThumbsUpGesture>();
                SpellCrafter.CraftingTimer.Continue();
            }
        }

        // Step 6 - THUMBS UP GESTURE
        private void OnThumbsUp(object sender, EventArgs e) {
            if (Tutorial.IsInStep(StepIndex.LAST)) {
                gameObject.SetActive(false);
                GameManager.Instance.GoToSettings();
            }
        }
        #endregion
    }
}