using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class SettingsState : State
    {
        public SettingsScreen settingsScreens;
        private void OnLetterGesture(object sender, char e)
        {
            Debug.Log(e);
            if (e == 'E')
            {
                settingsScreens.SelectSetting(0);
                Debug.Log("Settings Easy Selected");
            }
            else if (e == 'I')
            {
                settingsScreens.SelectSetting(1);
                Debug.Log("Settings Intermediate Selected");
            }
            else if (e == 'W')
            {
                settingsScreens.SelectSetting(2);
                Debug.Log("Settings Wizard Selected");
            }

            _player.Gestures.Enable<ThumbsUpGesture>();
        }

        private void OnThumbsUp(object sender, EventArgs e)
        {
            //_player.ChangeState(new PlayState(_player));
        }


        #region Inherited Methods
        public SettingsState(Player player) : base(player) { }

        public override void OnEnter()
        {
            _player.Gestures.DisableAllGestures();
            //settingsScreens = GameManager.Instance.SettingsScreen;
            settingsScreens.gameObject.SetActive(true);
            LetterGesture.PoseForm += OnLetterGesture;
            ThumbsUpGesture.PoseForm += OnThumbsUp;

            _player.Gestures.Enable<LetterGesture>();
        }

        public override void OnExit()
        {
            _player.Gestures.DisableAllGestures();
            settingsScreens.gameObject.SetActive(false);
            LetterGesture.PoseForm -= OnLetterGesture;
            ThumbsUpGesture.PoseForm -= OnThumbsUp;
        }
        #endregion
    }
}
