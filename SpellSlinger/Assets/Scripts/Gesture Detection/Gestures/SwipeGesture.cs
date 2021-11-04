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

		//private Tuple<float, float> EndingRange = new Tuple<float, float>(30.0f, 80.0f);
		//private Tuple<float, float> StartingRange = new Tuple<float, float>(250.0f, 330.0f);

		// Pose event
		public static EventHandler PoseEvent;

		//private bool isInRange(Tuple<float, float> range)
		//{
		//	float rotation = hand.transform.rotation.eulerAngles.x;

		//	if (rotation >= range.Item1 && rotation <= range.Item2)
		//		return true;
		//	else
		//		return false;
		//}

		#region Inherited Methods
		protected override bool PoseIsActive => hand.poseActive && hand.poseName == POSE && hand.transform.rotation.x < 0.0f;

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, EventArgs.Empty);
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			
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
