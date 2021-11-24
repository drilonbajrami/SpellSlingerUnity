using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TutorialState : State
    {
        public TutorialState(Player player) : base(player) { }

        private bool IsInStep(TutorialStepIndex index) => GameManager.Instance.Tutorial.IsInStep(index);

        //private void OnThumbsUp(object sender, EventArgs e)
        //{
        //    if (IsInStep(TutorialStepIndex.RESTARTORCONTINUE))
        //        Player.Instance.ChangeState(new PlayState());
        //    else if (IsInStep(TutorialStepIndex.SPELLSELECT))
        //    {
        //        Player.Instance.GestureCaster.Enable<LetterGesture>();
        //        Player.Instance.TutorialScreen.NextStep();
        //    }
        //    else if (IsInStep(TutorialStepIndex.HELPREMINDER))
        //    {
        //        Player.Instance.GestureCaster.Enable<CastGesture>();
        //        Player.Instance.TutorialScreen.NextStep();
        //    }
        //    else Player.Instance.TutorialScreen.NextStep();
        //}

        private void OnThumbsUp(object sender, EventArgs e)
        {
            _player.ChangeState(new PlayState(_player));
        }

        private void OnCraftGesture(object sender, EventArgs e)
        {
            if (GameManager.Instance.Tutorial.IsInStep(TutorialStepIndex.CRAFTGESTURE))
                _player.Gestures.Disable<LetterGesture>();
        }

        #region Inherited Methods
        public override void OnEnter()
        {
            // Enable tutorial
            GameManager.Instance.Tutorial.gameObject.SetActive(true);

            CraftGesture.PoseForm += OnCraftGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;
            _player.Gestures.Enable<CraftGesture>();     
        }

        public override void OnExit()
        {
            // Disable tutorial
            GameManager.Instance.Tutorial.gameObject.SetActive(false);

            _player.Gestures.Disable<ThumbsUpGesture>();
        }
        #endregion
    }
}