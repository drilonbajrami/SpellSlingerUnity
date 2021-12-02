using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private bool victoryScreen;

        private void Awake()
        {
            Health.Death += OnDeath;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Player.Instance.Gestures.DisableAllGestures();
            ThumbsUpGesture.PoseForm += OnThumbsUp;  
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();

            GameManager.Instance.DamageOverlay.SetActive(true);

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
            GameManager.Instance.ResetGame();
        }

        private void OnDeath(object sender, EventArgs e)
        {
            if(!victoryScreen)
                gameObject.SetActive(true);
            GameManager.Instance.ScorePanel.SetActive(false);
            GameManager.Instance.DamageOverlay.SetActive(false);
            GameManager.Instance.Overlay.SetActive(true);
            GameManager.Instance.SpellCrafter.ResetCrafting();
            GameManager.Instance.SpellProgress.SetActive(false);
        }
    }
}