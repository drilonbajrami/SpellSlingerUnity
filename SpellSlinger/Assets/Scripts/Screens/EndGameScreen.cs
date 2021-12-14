using UnityEngine;
using System;
using System.Collections;

namespace SpellSlinger
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private bool victoryScreen;

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private void Awake()
        {
            Health.Death += OnDeath;
            ScoreManager.KillsReached += OnKillsReached;
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            Player.Instance.Gestures.DisableAllGestures();
            GameManager.Instance.MoveToSpot("TutorialPosition");
            StartCoroutine(Wait(2));
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseDetected += OnThumbsUp;  
        }

        public void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            Player.Instance.Gestures.DisableAllGestures();
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