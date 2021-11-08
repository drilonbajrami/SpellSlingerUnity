using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class CastGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;
		[Header("HMD Transform")]
		[SerializeField] private Transform hmd;

		[Header("Pose name")]
		[SerializeField] private string POSE;

		[Header("Other")]
		[Tooltip("Distance threshold between the right hand and the head (HMD Device).\n" +
				 "Used to check for the CAST pose.")]
		[Range(0.2f, 0.5f)] [SerializeField] private float distanceThreshold = 0.4f;
		// Average retracted arm distance = 0.15 - 0.25
		// Average extended arm distance = 0.45 - 0.65

		public static EventHandler PoseEvent;

		// Keep track if completing cast pose is available
		private bool _canCompletePose = false;

		private float GetDistanceFromHandToHead()
		{
			return Vector2.Distance(new Vector2(hmd.position.x, hmd.position.z),
									new Vector2(hand.transform.position.x, hand.transform.position.z));
		}

		// Overridden Methods
		#region Overridden Inherited Methods
		protected override bool PoseIsActive => hand.poseActive && hand.poseName == POSE;

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, EventArgs.Empty);
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			if (!AreTrackersOn)
				_canCompletePose = true;
			else if (GetDistanceFromHandToHead() < distanceThreshold)
				_canCompletePose = true;
			else
			{
				_timer.Stop();
				_canCompletePose = false;
			}
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (_canCompletePose && PoseIsActive)
			{
				if (!AreTrackersOn) OnPose();
				else {
					if (GetDistanceFromHandToHead() > distanceThreshold)
						OnPose();
					else
					{
						_timer.Stop();
						_canCompletePose = false;
					}
				}
			}
		}

		protected override void OnInspectorChanges()
		{
			POSE = POSE.ToUpper();
		}
		#endregion
	}
}
