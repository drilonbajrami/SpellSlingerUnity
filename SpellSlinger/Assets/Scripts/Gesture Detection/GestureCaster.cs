using System;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		public bool withTrackers = false;

		#region Private Fields
		// Casting Signs
		private const string HELP_POSE = "HELP";
		private const string SWIPE_START_POSE = "SWIPE_START";
		private const string SWIPE_END_POSE = "SWIPE_END";
	
		[SerializeField] private HandEngine_Client rightHand;

		// Caching hand poses
		private string _rHandCurrentSwipePose;
		private string _rHandCurrentHelpPose;

		private float _rHandLastYAxisPosition;

		// Timers
		private Timer _swipePoseTimer;
		private Timer _helpPoseTimer;

		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] private float poseTimeSpan;

		[Range(0.1f, 0.3f)] [SerializeField] private float swipePoseTimeSpan = 0.2f;
		#endregion

		#region Event Handlers
		public static EventHandler SwipePoseEvent;
		public static EventHandler<bool> HelpPoseEvent;
		#endregion

		#region Event Raisers
		private void OnSwipePose() => SwipePoseEvent?.Invoke(this, EventArgs.Empty);
		private void OnHelpPose(bool open) => HelpPoseEvent?.Invoke(this, open);
		#endregion

		#region UNITY Methods
		private void Start()
		{
			SetupTimers(poseTimeSpan);
		}

		private void FixedUpdate()
		{
			if (rightHand.poseActive && !_swipePoseTimer.Running)
				_swipePoseTimer.Start();

			if (rightHand.poseActive && !_helpPoseTimer.Running)
				_helpPoseTimer.Start();

			_swipePoseTimer.UpdateTimer(Time.fixedDeltaTime);
			_helpPoseTimer.UpdateTimer(Time.fixedDeltaTime);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Set up separate and common timers for both hands.
		/// </summary>
		private void SetupTimers(float poseTimeSpan)
		{
			_swipePoseTimer = new Timer(swipePoseTimeSpan);
			_swipePoseTimer.TimerStart += SwipePoseTimerStart;
			_swipePoseTimer.TimerEnd += SwipePoseTimerEnd;

			_helpPoseTimer = new Timer(0.3f);
			_helpPoseTimer.TimerEnd += HelpPoseTimerEnd;
			_helpPoseTimer.TimerStart += HelpPoseTimerStart;
		}
		#endregion

		#region Event Subscriber Methods
		private void HelpPoseTimerEnd(object sender, EventArgs e)
		{
			if (_rHandCurrentHelpPose == rightHand.poseName && rightHand.poseActive)
			{
				if (rightHand.transform.position.y - _rHandLastYAxisPosition > 0.5f)
					OnHelpPose(true);
				else if (_rHandLastYAxisPosition - rightHand.transform.position.y > 0.5f)
					OnHelpPose(false);
			}
		}

		private void HelpPoseTimerStart(object sender, EventArgs e)
		{
			if (rightHand.poseActive)
			{
				_rHandCurrentHelpPose = rightHand.poseName;
				_rHandLastYAxisPosition = rightHand.transform.position.y;
			}
		}

		/// <summary>
		/// On Timer Finish subscriber method for right hand swipe pose
		/// </summary>
		private void SwipePoseTimerEnd(object source, EventArgs e)
		{
			if (_rHandCurrentSwipePose == SWIPE_START_POSE && rightHand.poseName == SWIPE_END_POSE && rightHand.poseActive)
				OnSwipePose();
		}

		/// <summary>
		/// On Timer Reset subscriber method for right hand swipe pose
		/// </summary>
		private void SwipePoseTimerStart(object source, EventArgs e)
		{
			if (rightHand.poseActive)
				_rHandCurrentSwipePose = rightHand.poseName;
			else
				_rHandCurrentSwipePose = string.Empty;
		}
		#endregion
	}
}