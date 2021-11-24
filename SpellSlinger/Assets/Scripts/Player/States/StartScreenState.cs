using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreenState : State
    {
        public StartScreenState(Player player) : base(player) { }

        private void OnThumbsUp(object sender, EventArgs e)
        {
            _player.ChangeState(new TutorialState(_player));
        }

        #region Inherited Methods
        public override void OnEnter()
        {
            // Enable Start Screen and Overlay
            GameManager.Instance.StartScreen.SetActive(true);
            GameManager.Instance.Overlay.SetActive(true);

            ThumbsUpGesture.PoseForm += OnThumbsUp;
            _player.Gestures.Enable<ThumbsUpGesture>(); 
        }

        public override void OnExit()
        {
            // Disable Start Screen
            GameManager.Instance.StartScreen.SetActive(false);

            _player.Gestures.Disable<ThumbsUpGesture>();
        }
        #endregion
    }
}