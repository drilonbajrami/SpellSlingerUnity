using System;
using UnityEngine;

namespace SpellSlinger
{
	public class LetterGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		// Pose event
		public static EventHandler<char> PoseEvent;

		// Overridden Methods
		#region Inherited Methods
		protected override bool PoseIsActive => hand.poseActive;

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, char.Parse(_lastPose));
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			_lastPose = hand.poseName;
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (PoseIsActive && _lastPose == hand.poseName && _lastPose.Length <= 1)
				OnPose();
		}
		#endregion
	}
}
