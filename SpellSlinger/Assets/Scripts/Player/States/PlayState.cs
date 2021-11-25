using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class PlayState : State
    {
        public PlayState(Player player) : base(player) { }

        #region Inherited Methods
        public override void OnEnter()
        {
            _player.Gestures.DisableAllGestures();
            _player.Gestures.Enable<CraftGesture>();
            _player.Gestures.Enable<CastGesture>();
            GameManager.Instance.Overlay.SetActive(false);
            GameObject tutorialPosition = GameObject.Find("StartPosition");
            _player.transform.position = tutorialPosition.transform.position;
            _player.transform.rotation = tutorialPosition.transform.rotation;
        }

        public override void OnExit()
        {
            _player.Gestures.DisableAllGestures();
        }
        #endregion
    }
}
