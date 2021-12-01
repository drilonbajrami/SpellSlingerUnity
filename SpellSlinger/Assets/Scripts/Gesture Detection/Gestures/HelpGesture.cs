using System;
using UnityEngine;

namespace SpellSlinger
{
	public class HelpGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		[Header("Pose name")]
		[SerializeField] private string POSE;

		// Keep track if panel is open or not
		private bool isOpen = false;

		// Pose event
		public static event EventHandler<bool> PoseForm;

		#region Inherited Methods
		protected override bool PoseIsActive => Player.NO_GLOVES ?
												(!isOpen && Input.GetKey(KeyCode.LeftAlt)) || (isOpen && !Input.GetKey(KeyCode.LeftAlt))      // Keyboard
												: (!isOpen && hand.poseActive && hand.poseName == POSE) || (isOpen && hand.poseName != POSE); // Gloves

		protected override void OnPose() => PoseForm?.Invoke(this, isOpen);

		protected override void PoseStart(object sender, EventArgs e)
		{
			// This will close the help panel if currently open
			if (isOpen)
			{
				isOpen = false;
				_timer.Stop();
				OnPose();
			}
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			// If help panel currently closed
			if (!isOpen && 
				(Input.GetKey(KeyCode.LeftAlt) ||				// Keyboard
				(hand.poseActive && hand.poseName == POSE)))	// Gloves
			{
				isOpen = true;
				OnPose();
			}
		}

		protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
		#endregion
	}
}