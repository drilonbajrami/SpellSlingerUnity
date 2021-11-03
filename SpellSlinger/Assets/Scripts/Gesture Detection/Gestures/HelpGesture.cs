using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class HelpGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		[Header("Pose name")]
		[SerializeField] private string POSE;

		[Header("Other")]
		[Tooltip("Distance threshold between the right hand and the head (HMD Device).\n" +
				 "Used to check for the CAST pose.")]
		[Range(0.2f, 0.5f)] [SerializeField] private float distanceThreshold = 0.5f;

		private float _handLastYAxisPosition;
		private bool open = true;

		// Pose event
		public static EventHandler<bool> PoseEvent;

		#region Inherited Methods
		protected override bool PoseIsActive => hand.poseActive && hand.poseName == POSE;

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, open);
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			_handLastYAxisPosition = hand.transform.position.y;
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (PoseIsActive)
			{
				if (hand.transform.position.y - _handLastYAxisPosition > distanceThreshold)
					open = true;
				else if (_handLastYAxisPosition - hand.transform.position.y > distanceThreshold)
					open = false;

				OnPose();
			}
		}

		protected override void OnInspectorChanges()
		{
			POSE = POSE.ToUpper();
		}
		#endregion
	}
}
