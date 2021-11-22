using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class ThumbsUpGesture : Gesture
    {
        [Header("Hand")]
        [SerializeField] private HandEngine_Client hand;

        [Header("Pose Name")]
        [SerializeField] private string POSE;

        public static EventHandler PoseForm;

        // Overridden Methods
        #region Inherited Methods
        protected override bool PoseIsActive => Player.NO_GLOVES ? Input.GetKey(KeyCode.KeypadEnter)
                                                                 : hand.poseActive && hand.poseName == POSE;

        protected override void OnPose() => PoseForm?.Invoke(this, EventArgs.Empty);

        protected override void PoseEnd(object sender, EventArgs e)
        {
            if (PoseIsActive)
                OnPose();
        }

        protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
        #endregion
    }
}
