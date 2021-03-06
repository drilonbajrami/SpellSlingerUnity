using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace SpellSlinger
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }

        public TMP_Text scoreText;
        public static event EventHandler KillsReached;

        private int kills;
        [SerializeField] private int killsToWin = 10;

        private void Update()
        {
            scoreText.text = Score.ToString();

            if (kills == killsToWin)
            {
                kills = 0;
                KillsReached?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateScore(int score)
        {
            kills++;
            Score += score;
        }

        public void ResetScore()
        {
            kills = 0;
            Score = 0;
        }
    }
}
