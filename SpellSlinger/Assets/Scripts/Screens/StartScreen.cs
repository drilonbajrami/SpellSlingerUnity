using System;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreen : MonoBehaviour
    {
        private void OnEnable() => ThumbsUpGesture.PoseDetected += OnThumbsUp;
        public void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            AudioManager.Instance.Play("Confirm");
            GameManager.Instance.GoToTutorial();
        }
    }
}