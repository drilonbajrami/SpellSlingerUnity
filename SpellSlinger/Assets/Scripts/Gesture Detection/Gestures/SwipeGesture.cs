using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
	public enum SwipeDirection { LEFT, RIGHT }

	public class SwipeDirections : EventArgs
	{
		public SwipeDirection defaultDirection { get; }
		public SwipeDirection currentDirection { get; }

		public SwipeDirections(SwipeDirection _defaultDirection, SwipeDirection _currentDirection)
		{
			defaultDirection = _defaultDirection;
			currentDirection = _currentDirection;
		}
	}

	public class SwipeGesture : Gesture
	{
		[Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		[Header("Pose names")]
		[SerializeField] private string POSE_START;
		[SerializeField] private string POSE_END;

		// TO DO: change the sweep poses in Hand Engine to 
		// SWEEPLEFT, SWEEPRIGHT

		[Header("Other")]
		[Tooltip("Set swiping to bidirectional behaviour on/off.")]
		[SerializeField] private bool _biDirectional = false;
		[Tooltip("Set default swipe direction when bidirectional behavior is off.")]
		[SerializeField] private SwipeDirection swipeDefaultDirection = SwipeDirection.RIGHT;
		private SwipeDirection swipeCurrentDirection = SwipeDirection.RIGHT;
		private SwipeDirections swipeDirections;

		// Pose event
		public static EventHandler<SwipeDirections> PoseEvent;

		#region Inherited Methods
		protected override bool PoseIsActive => hand.poseActive && (hand.poseName == POSE_START || hand.poseName == POSE_END);

		protected override void OnPose()
		{
			PoseEvent?.Invoke(this, new SwipeDirections(swipeDefaultDirection, swipeCurrentDirection));
		}

		protected override void PoseStart(object sender, EventArgs e)
		{
			_lastPose = hand.poseName;
		}

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (PoseIsActive) {
				if (!_biDirectional) {
					if (_lastPose == POSE_START && hand.poseName == POSE_END){
						swipeCurrentDirection = swipeDefaultDirection;
						OnPose();
					}				
				} else {
					if (_lastPose == POSE_START && hand.poseName == POSE_END)
					{
						swipeCurrentDirection = SwipeDirection.RIGHT;
						OnPose();
					}
					else if (_lastPose == POSE_END && hand.poseName == POSE_START)
					{
						swipeCurrentDirection = SwipeDirection.LEFT;
						OnPose();
					}
				}
			}
		}

		protected override void OnInspectorChanges()
		{
			POSE_START = POSE_START.ToUpper();
			POSE_END = POSE_END.ToUpper();
			_biDirectional = !_biDirectional;
		}
		#endregion
	}
}
