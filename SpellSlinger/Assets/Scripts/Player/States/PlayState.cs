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
            // Disable all gestures for safety
            _player.Gestures.DisableAllGestures();

            // Enable Craft and Cast gestures (these are essential)
            _player.Gestures.Enable<CraftGesture>();
            _player.Gestures.Enable<CastGesture>();

            //GameManager.Instance.Overlay.SetActive(false);

            // Move to game start position
            GameObject tutorialPosition = GameObject.Find("StartPosition");
            _player.transform.position = tutorialPosition.transform.position;
            _player.transform.rotation = tutorialPosition.transform.rotation;

            // Enable Enemy Spawner
            GameManager.Instance.Spawner.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            _player.Gestures.DisableAllGestures();
        }
        #endregion
    }
}
