using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            ThumbsUpGesture.PoseForm += OnThumbsUp;
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();

            // Move to start screen & tutorial position
            GameObject tutorialPosition = GameObject.Find("TutorialPosition");
            Player.Instance.transform.position = tutorialPosition.transform.position;
            Player.Instance.transform.rotation = tutorialPosition.transform.rotation;
        }

        public void OnDisable()
        {
            ThumbsUpGesture.PoseForm -= OnThumbsUp;
            Player.Instance.Gestures.Disable<ThumbsUpGesture>();
        }

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            GameManager.Instance.TutorialScreen.SetActive(true);
        }
    }
}
