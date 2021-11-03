using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class CraftGesture : Gesture
	{
		[Header("Hands")]
		[SerializeField] private HandEngine_Client leftHand;
		[SerializeField] private HandEngine_Client rightHand;

		[Header("Pose names")]
		[Tooltip("Left hand pose name")]
		[SerializeField] private string POSE_LEFT;
		[Tooltip("Right hand pose name")]
		[SerializeField] private string POSE_RIGHT;

		[Header("Other")]
		[Tooltip("Distance threshold between two hands, used for checking if " +
				 "hands are close enough during some specific spells/gestures.\n" +
				 "Optimal distance threshold is around 0.25f units")]
		[Range(0.2f, 0.5f)] [SerializeField] private float distanceThreshold = 0.25f;
		
		// Pose event
		public static EventHandler PoseEvent;

		private string GetHandPoses() => leftHand.poseName + rightHand.poseName;

		// Override Methods
		#region Inherited Methods
		protected override bool PoseIsActive => leftHand.poseActive && 
												rightHand.poseActive && 
												GetHandPoses() == POSE_LEFT + POSE_RIGHT;

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, EventArgs.Empty);
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			_lastPose = GetHandPoses();
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (_lastPose == GetHandPoses()) {
				if (AreTrackersOn) {
					float distance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
					if(distance < distanceThreshold) OnPose();
				}
				else OnPose();
			}
		}

		protected override void OnInspectorChanges()
		{
			POSE_LEFT = POSE_LEFT.ToUpper();
			POSE_RIGHT = POSE_RIGHT.ToUpper();
		}
		#endregion
	}
}
