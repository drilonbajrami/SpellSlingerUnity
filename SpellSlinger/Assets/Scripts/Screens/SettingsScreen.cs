using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace SpellSlinger
{
    public class SettingsScreen : MonoBehaviour
    {
        public GameObject continueHint;
        public List<SettingsUISelector> settingSelectors;

        [Space(10)]
        public List<GameSettingSO> avaliableGameSettings;

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private void OnEnable() {

            StartCoroutine(Wait(2));
             LetterGesture.PoseDetected += OnLetterGesture;
            ThumbsUpGesture.PoseDetected += OnThumbsUp;          
        }

        private void OnDisable() {
            LetterGesture.PoseDetected -= OnLetterGesture;
            ThumbsUpGesture.PoseDetected -= OnThumbsUp;
            continueHint.SetActive(false);
        }

        public void SelectSetting(int index) {
            for (int i = 0; i < settingSelectors.Count; i++) {
                if (i == index) { 
                    settingSelectors[i].Select();
                    AudioManager.Instance.Play("Confirm");
                    GameManager.Instance.ApplyGameSettings(avaliableGameSettings[i]);
                } else settingSelectors[i].Deselect();
            }

            if (!continueHint.activeSelf) continueHint.SetActive(true);
        }

        private void OnLetterGesture(object sender, char e)
        {
            if (e == 'E') SelectSetting(0);
            else if (e == 'I') SelectSetting(1);
            else if (e == 'W') SelectSetting(2);

            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
        }

        private void OnThumbsUp(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            AudioManager.Instance.Play("Confirm");
            GameManager.Instance.Play();
        }
    }
}