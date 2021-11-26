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
            //_player.ChangeState(new TutorialState(_player));
        }

        #region Inherited Methods
        public StartScreenState(Player player) : base(player) { }

        public override void OnEnter()
        {
            // Enable Start Screen and Overlay
            //GameManager.Instance.StartScreen.SetActive(true);
            //GameManager.Instance.Overlay.SetActive(true);

            // Only gesture needed for start screen
            ThumbsUpGesture.PoseForm += OnThumbsUp;
            _player.Gestures.Enable<ThumbsUpGesture>();

            // Move to start screen & tutorial position
            GameObject tutorialPosition = GameObject.Find("TutorialPosition");
            _player.transform.position = tutorialPosition.transform.position;
            _player.transform.rotation = tutorialPosition.transform.rotation;
        }

        public override void OnExit()
        {
            // Disable Start Screen
            //GameManager.Instance.StartScreen.SetActive(false);

            _player.Gestures.Disable<ThumbsUpGesture>();
        }
        #endregion
    }
}