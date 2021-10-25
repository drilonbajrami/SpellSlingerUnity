using System;
using UnityEngine;

namespace SpellSlinger
{
    public class GestureCaster : MonoBehaviour
    {
        // HandEngine clients for reading pose data
		[SerializeField] private HandEngine_Client _leftHand;
        [SerializeField] private HandEngine_Client _rightHand;

        // Caching hand poses
        private string _leftHandCurrentPose;
        private string _rightHandCurrentPose;

        private float leftHandPoseTimer = 0.0f;
        private float rightHandPoseTimer = 0.0f;

        [Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\nIt is recommended to keep this time span at 0.5 seconds.")]
        [Range(0.1f, 2.0f)][SerializeField] private float poseTimeSpan;

        // Events
        public static event EventHandler<string> LeftHandGesture;
        public static event EventHandler<string> RightHandGesture;

		private void FixedUpdate()
        {
            // Left pose timer
            if (leftHandPoseTimer <= 0.0f)
                _leftHandCurrentPose = _leftHand.poseName;

            // Right pose timer
            if (rightHandPoseTimer <= 0.0f)
                _rightHandCurrentPose = _rightHand.poseName;

            // Increment timers
            leftHandPoseTimer += Time.fixedDeltaTime;
            rightHandPoseTimer += Time.fixedDeltaTime;

            // Left Hand
            if (leftHandPoseTimer >= poseTimeSpan)
                if (_leftHandCurrentPose == _leftHand.poseName && _leftHand.poseActive)
                    OnLeftHandGesture();
                else
                    ResetLeftHandPoseTimer();

            // Right Hand
			if (rightHandPoseTimer >= poseTimeSpan)
				if (_rightHandCurrentPose == _rightHand.poseName && _rightHand.poseActive)
					OnRightHandGesture();
				else
					ResetRightHandPoseTimer();
		}

        // Event raiser for Left Hand Gestures
		protected virtual void OnLeftHandGesture()
		{
            ResetLeftHandPoseTimer();
            LeftHandGesture?.Invoke(this, _leftHandCurrentPose);
        }

        // Event raiser for Right Hand Gestures
        protected virtual void OnRightHandGesture()
		{
            ResetRightHandPoseTimer();
            RightHandGesture?.Invoke(this, _rightHandCurrentPose);
		}

		private void ResetLeftHandPoseTimer() => leftHandPoseTimer = 0.0f;
        private void ResetRightHandPoseTimer() => rightHandPoseTimer = 0.0f;
	}
}