using UnityEngine;
using System;
using System.Collections;

namespace SpellSlinger
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private bool victoryScreen;

        private IEnumerator Wait(float seconds) {   
            yield return new WaitForSeconds(seconds);     
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseDetected += OnThumbsUp;
        }

        private void Awake()
        {
            GemStones.GemStonesDestroyed += OnGemStonesDestroyed;
            ScoreManager.KillsReached += OnKillsReached;
            gameObject.SetActive(false);
        }

        private void OnEnable() => StartCoroutine(Wait(2));
        public void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            Player.Instance.Gestures.DisableAllGestures();
            gameObject.SetActive(false);
            GameManager.Instance.GoToStart();
        }

        private void OnGemStonesDestroyed(object sender, EventArgs e)
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