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
            ScoreManager.KillsReached += OnKillsReached;
            gameObject.SetActive(false);
        }

        private void OnEnable() {

            GameManager.Instance.MoveToSpot("TutorialPosition");
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseDetected += OnThumbsUp;  
        }

        public void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GoToStart();
        }

        private void OnDeath(object sender, EventArgs e)
        {
            if(!victoryScreen) gameObject.SetActive(true);
            GameManager.Instance.OnGameEnd();
        }

        private void OnKillsReached(object sender, EventArgs e)
        {
            if (victoryScreen) gameObject.SetActive(true);
            GameManager.Instance.OnGameEnd();    
        }
    }
}