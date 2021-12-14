using System;
using System.Collections;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreen : MonoBehaviour
    {
        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private void OnEnable()
        {
            Player.Instance.Gestures.DisableAllGestures();
            StartCoroutine(Wait(2));
            
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseDetected += OnThumbsUp;
        }
        private void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            AudioManager.Instance.Play("Confirm");
            GameManager.Instance.GoToTutorial();
        }
    }
}