using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
	public class CastGesture : Gesture
	{
        [Header("Hand & Head Transform")]
		[SerializeField] private HandEngine_Client hand;
		[SerializeField] private Transform head;
		private Transform handT;

		[Header("Pose name")]
		[SerializeField] private string POSE;

		[Header("Other")]
		[Tooltip("Distance threshold between the right hand and the head (HMD Device).\n" +
				 "Used to check for the CAST pose.\n" +
				 "Average retracted arm distance = 0.15 - 0.25 units.\n" +
				 "Average extended arm distance = 0.45 - 0.65 units.")]
		[Range(0.2f, 0.5f)] [SerializeField] private float distanceThreshold = 0.4f;

		// Keep track if completing cast pose is available
		private bool _canCompletePose = false;

        // Pose event
        public static event EventHandler PoseForm;

        #region Private Properties & Methods
        private float DistanceFromHandToHead => Vector2.Distance(
											new Vector2(head.position.x, head.position.z),	// Head
											new Vector2(handT.position.x, handT.position.z)	// Hand
											);
		
		private bool IsHandCloseToHead => DistanceFromHandToHead < distanceThreshold;

        private void Start() => handT = hand.transform; // Cache hand transform

		private void ResetPoseCheck()
		{
			_timer.Stop();
			_canCompletePose = false;
		}
		#endregion

		#region Inherited Methods
		protected override bool PoseIsActive => Player.NO_GLOVES ? // Keyboard or Gloves
												Input.GetKey(KeyCode.Space) :			
												hand.poseActive && hand.poseName == POSE;	

		protected override void OnPose() => PoseForm?.Invoke(this, EventArgs.Empty);

		protected override void PoseStart(object sender, EventArgs e)
		{
			if (Player.NO_GLOVES) {
				_canCompletePose = false;
				PoseForm?.Invoke(this, EventArgs.Empty);
			}	
			else if (!AreTrackersOn || IsHandCloseToHead) _canCompletePose = true;
			else ResetPoseCheck();
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (_canCompletePose && PoseIsActive) {
				if (!AreTrackersOn) OnPose();
				else {
					if (!IsHandCloseToHead) OnPose();
					else ResetPoseCheck();
				}
			}
		}

		protected override void OnInspectorChanges() => POSE = POSE.ToUpper();
		#endregion
	}
}