using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class SettingsScreen : MonoBehaviour
    {
        public GameObject thumbsUpContinuePanel;
        public List<SettingsUISelector> settingSelectors;

        [Space(10)]
        public List<GameSettingScriptableObject> avaliableGameSettings = new List<GameSettingScriptableObject>();

        #region UNITY Methods
        private void OnEnable()
        {
            Player.Instance.Gestures.DisableAllGestures();
            LetterGesture.PoseForm += OnLetterGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;
            Player.Instance.Gestures.Enable<LetterGesture>();
        }

        private void OnDisable()
        {
            LetterGesture.PoseForm -= OnLetterGesture;
            ThumbsUpGesture.PoseForm -= OnThumbsUp;
        }
        #endregion

        public void SelectSetting(int index)
        {
            for (int i = 0; i < settingSelectors.Count; i++)
            {
                if (i == index)
                { 
                    settingSelectors[i].Select();
                    GameManager.Instance.ApplyGameSettings(avaliableGameSettings[i]);
                }    
                else settingSelectors[i].Deselect();
            }

            if (!thumbsUpContinuePanel.activeSelf)
                thumbsUpContinuePanel.SetActive(true);
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
            GameManager.Instance.Spawner.gameObject.SetActive(true);
            GameManager.Instance.Overlay.SetActive(false);

            Player.Instance.Gestures.Enable<CraftGesture>();
            Player.Instance.Gestures.Enable<CastGesture>();

            // Move to start screen & tutorial position
            GameObject startingPosition = GameObject.Find("StartPosition");
            Player.Instance.transform.position = startingPosition.transform.position;
            Player.Instance.transform.rotation = startingPosition.transform.rotation;

            GameManager.Instance.SpellCrafter.Toggle(true);
        }
    }
}