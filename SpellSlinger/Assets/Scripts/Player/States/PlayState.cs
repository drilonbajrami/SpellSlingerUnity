using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class PlayState : State
    {
        #region Inherited Methods
        public override void OnStateStart()
        {
            Player.Instance.OverlayCanvas.SetActive(false);
            Player.Instance.GestureCaster.DisableAllGestures();
            Player.Instance.GestureCaster.Enable<CraftGesture>();
        }

        public override void OnStateEnd()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
