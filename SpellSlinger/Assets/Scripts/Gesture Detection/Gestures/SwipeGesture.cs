using UnityEngine;
using System;

namespace SpellSlinger
{
	public class SwipeGesture : Gesture
	{
        [Header("Hand")]
		[SerializeField] private HandEngine_Client hand;
		private Transform handT;

		[Header("Pose name")]
		[SerializeField] private string POSE;

        // Pose event
        public static event EventHandler PoseForm;

        private void Start() => handT = hand.transform;

		public bool HandPalmFacingUp => handT.rotation.x < 0.0f;

		#region Inherited Methods
		protected override bool PoseIsActive => Player.NO_GLOVES ? 
												Input.GetKey(KeyCode.LeftControl)	// Keyboard
												: hand.poseActive &&				// Gloves
												  hand.poseName == POSE && 
												  HandPalmFacingUp; 

		protected override void OnPose() => PoseForm?.Invoke(this, EventArgs.Empty);

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if ((hand.poseActive && hand.poseName == POSE && !HandPalmFacingUp)	// Gloves
				|| !Input.GetKey(KeyCode.LeftControl))							// Keyboard
				OnPose();
		}

		protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
		#endregion
	}
}
