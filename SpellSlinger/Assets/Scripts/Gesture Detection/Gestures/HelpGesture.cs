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

		private bool open = false;

		// Pose event
		public static EventHandler<bool> PoseForm;

		protected override bool PoseIsActive => Player.NO_GLOVES ? (!open && Input.GetKey(KeyCode.Space)) || (open && !Input.GetKey(KeyCode.Space))
												: (!open && hand.poseActive && hand.poseName == POSE) || (open && hand.poseName != POSE);

		#region Inherited Methods
		//protected override bool PoseIsActive => (!open && hand.poseActive && hand.poseName == POSE) || (open && hand.poseName != POSE);

		protected override void OnPose() => PoseForm?.Invoke(this, open);

		protected override void PoseStart(object sender, EventArgs e)
		{
			// If open is true then negate it and raise event
			// This will close the help menu
			if (open)
			{
				open = false;
				_timer.Stop();
				OnPose();
			}
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			// If pose detection has started and continued
			// Check if the actual pose the correct one and the open is false (menu not open yet)
			if (!open && hand.poseActive && hand.poseName == POSE)
			{
				open = true;
				OnPose();
			}
			else if (!open && Input.GetKey(KeyCode.Space))
			{
				open = true;
				OnPose();
			}
		}

		protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
		#endregion
	}
}
