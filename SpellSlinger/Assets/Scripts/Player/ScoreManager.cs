using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpellSlinger
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }

        public TMP_Text scoreText;

        private void Update()
        {
            scoreText.text = Score.ToString();
        }

        public void UpdateScore(int score) => Score += score;
        public void ResetScore() => Score = 0;
    }
}
