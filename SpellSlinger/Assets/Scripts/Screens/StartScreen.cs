using System;
using System.Collections;
using UnityEngine;

namespace SpellSlinger
{
    public class StartScreen : MonoBehaviour
    {
        private IEnumerator Wait(float seconds) { 
            yield return new WaitForSeconds(seconds);
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
            ThumbsUpGesture.PoseDetected += OnThumbsUp;
        }

        private void OnEnable() => StartCoroutine(Wait(2));
        private void OnDisable() => ThumbsUpGesture.PoseDetected -= OnThumbsUp;

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            AudioManager.Instance.Play("Confirm");
            GameManager.Instance.GoToTutorial();
        }
    }
}