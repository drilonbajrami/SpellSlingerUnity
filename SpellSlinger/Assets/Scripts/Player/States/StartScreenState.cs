using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreenState : State
    {
        private void OnThumbsUp(object sender, EventArgs e)
        {
            Player.Instance.ChangeState(new TutorialState());
        }

        #region Inherited Methods
        public override void OnStateStart()
        {
            Player.Instance.StartScreen.SetActive(true);
            Player.Instance.OverlayCanvas.SetActive(true);
            Player.Instance.GestureCaster.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseForm += OnThumbsUp;
        }

        public override void OnStateEnd()
        {
            Player.Instance.StartScreen.SetActive(false);
            //Player.Instance.OverlayCanvas.SetActive(false);
            ThumbsUpGesture.PoseForm -= OnThumbsUp;
        }
        #endregion
    }
}