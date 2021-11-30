using System;
using UnityEngine;

namespace SpellSlinger
{
	public class CraftGesture : Gesture
	{
        [Header("Hands")]
		[SerializeField] private HandEngine_Client leftHand;
		[SerializeField] private HandEngine_Client rightHand;
		private Transform lHand;
		private Transform rHand;

		[Header("Pose names")]
		[Tooltip("Left hand pose name")]
		[SerializeField] private string POSE_LEFT;
		[Tooltip("Right hand pose name")]
		[SerializeField] private string POSE_RIGHT;
		private string POSE;

		[Header("Other")]
		[Tooltip("Distance threshold between two hands, used for checking if " +
				 "hands are close enough during some specific spells/gestures.\n" +
				 "Optimal distance threshold is around 0.25f units")]
		[Range(0.2f, 0.5f)] [SerializeField] private float distanceThreshold = 0.25f;

        // Pose event
        public static event EventHandler PoseForm;

        #region Private Properties & Methods
        /// <summary>
        /// Returns a co-joined string of both hands pose names
        /// </summary>
        private string HandPoses => leftHand.poseName + rightHand.poseName;
		private float DistanceBetweenHands => Vector3.Distance(lHand.position, rHand.position);
		private bool HandsAreCloseToEachOther => DistanceBetweenHands < distanceThreshold;
        #endregion

        #region Inherited Methods
        protected override bool PoseIsActive => Player.NO_GLOVES ? // Keyboard or Gloves
												Input.GetKey(KeyCode.Semicolon) : 
												leftHand.poseActive && rightHand.poseActive && POSE == HandPoses;

		protected override void OnPose() => PoseForm?.Invoke(this, EventArgs.Empty);

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (PoseIsActive) {
				if (AreTrackersOn && HandsAreCloseToEachOther) OnPose();
				else OnPose();
			}
		}

		protected override void OnInspectorChanges()
		{
			POSE_LEFT = POSE_LEFT.ToUpper();
			POSE_RIGHT = POSE_RIGHT.ToUpper();
			POSE = POSE_LEFT + POSE_RIGHT;
		}
		#endregion
	}
}
