using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class TutorialState : State
    {
        private bool IsInStep(TutorialStepIndex index) => Player.Instance.TutorialScreen.IsInStep(index);

        private void OnThumbsUp(object sender, EventArgs e)
        {
            if (IsInStep(TutorialStepIndex.RESTARTORCONTINUE))
                Player.Instance.ChangeState(new PlayState());
            else if (IsInStep(TutorialStepIndex.SPELLSELECT))
            {
                Player.Instance.GestureCaster.Enable<LetterGesture>();
                Player.Instance.TutorialScreen.NextStep();
            }
            else if (IsInStep(TutorialStepIndex.HELPREMINDER))
            {
                Player.Instance.GestureCaster.Enable<CastGesture>();
                Player.Instance.TutorialScreen.NextStep();
            }
            else Player.Instance.TutorialScreen.NextStep();
        }

        private void OnCraftGesture(object sender, EventArgs e)
        {
            if (Player.Instance.TutorialScreen.IsInStep(TutorialStepIndex.CRAFTGESTURE))
                Player.Instance.GestureCaster.Disable<LetterGesture>();
        }

        #region Inherited Methods
        public override void OnStateStart()
        {
            Player.Instance.OverlayCanvas.SetActive(true);
            Player.Instance.TutorialScreen.gameObject.SetActive(true);
          
            Player.Instance.GestureCaster.Enable<CraftGesture>();

            CraftGesture.PoseForm += OnCraftGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;
        }

        public override void OnStateEnd()
        {
            Player.Instance.TutorialScreen.gameObject.SetActive(false);
            Player.Instance.OverlayCanvas.SetActive(false);
        }
        #endregion
    }
}