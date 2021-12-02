using System;
using UnityEngine;

namespace SpellSlinger
{
    public class ThumbsUpGesture : Gesture
    {
        [Header("Hand")]
        [SerializeField] private HandEngine_Client hand;

        [Header("Pose Name")]
        [SerializeField] private string POSE;

        // Pose event
        public static EventHandler PoseForm;

        #region Inherited Methods
        protected override bool PoseIsActive => Player.NO_GLOVES ? Input.GetKey(KeyCode.K)
                                                                 : hand.poseActive && hand.poseName == POSE;

        protected override void OnPose() => PoseForm?.Invoke(this, EventArgs.Empty);

        protected override void PoseEnd(object sender, EventArgs e)
        {
            if (PoseIsActive) OnPose();
        }

        protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
        #endregion
    }
}