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
            Player.Instance.OverlayCanvas.SetActive(false);
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.Gestures.Enable<CraftGesture>();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
