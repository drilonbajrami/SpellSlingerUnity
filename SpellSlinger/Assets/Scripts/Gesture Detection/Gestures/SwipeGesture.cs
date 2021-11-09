using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
	public class SwipeGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		[Header("Pose name")]
		[SerializeField] private string POSE;

		// Pose event
		public static EventHandler PoseForm;

		#region Inherited Methods
		protected override bool PoseIsActive => hand.poseActive && hand.poseName == POSE && hand.transform.rotation.x < 0.0f;

		protected override void OnPose()
		{
			PoseForm?.Invoke(this, EventArgs.Empty);
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (hand.poseActive && hand.poseName == POSE && hand.transform.rotation.x > 0.0f)
				OnPose();
		}

		protected override void OnInspectorChanges()
		{
			POSE= POSE.ToUpper();
		}
		#endregion
	}
}
