using UnityEngine;
using System;

namespace SpellSlinger
{
	public abstract class Gesture : MonoBehaviour
	{
		/// <summary>
		/// Keep track if gesture is active or not
		/// Can be used to turn off gesture recognition when not needed
		/// </summary>
		private bool _isOn = true;
		protected bool IsOn { get { return _isOn; } }

		[Header("Tracker availability")]
		[SerializeField] protected bool _areTrackersOn;
		protected bool AreTrackersOn { get { return _areTrackersOn; } }

		[Header("Pose timer duration")]
		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] protected float _poseTimeSpan = 0.5f;

		// Cache last known pose name
		protected string _lastPose;

		// Pose Timer
		protected Timer _timer;

		#region UNITY
		private void Start()
		{
			// Setup the timer and subscribe to timer events
			_timer = new Timer(_poseTimeSpan);
			_timer.TimerStart += PoseStart;
			_timer.TimerEnd += PoseEnd;	
		}		

		private void FixedUpdate()
		{
			if (!_isOn) return;

			// Check if timer is running or not and if hands currently have active poses
			// If timer is not running and hands are in active poses then start timer
			if (!_timer.Running && PoseIsActive)
				_timer.Start();

			_timer.UpdateTimer(Time.fixedDeltaTime);
		}

		private void OnValidate()
		{
			if(_timer != null)
				_timer.ChangeInterval(_poseTimeSpan);

			OnInspectorChanges();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Change the pose time span
		/// </summary>
		public void ChangePoseTimeSpan(float seconds)
		{
			_poseTimeSpan = seconds;
		}

		/// <summary>
		/// Enables gesture
		/// </summary>
		public void TurnOn() => _isOn = true;

		/// <summary>
		/// Disables gesture
		/// </summary>
		public void TurnOff() => _isOn = false;
		#endregion

		#region Abstract Methods
		/// <summary>
		/// Returns the state of the hand poses
		/// </summary>
		abstract protected bool PoseIsActive { get; }

		/// <summary>
		/// On Timer Start event subscriber method
		/// </summary>
		protected abstract void PoseStart(object sender, EventArgs e);

		/// <summary>
		/// On Timer End event subscriber method
		/// </summary>
		protected abstract void PoseEnd(object sender, EventArgs e);

		/// <summary>
		/// Event raiser method when pose is detected
		/// </summary>
		protected abstract void OnPose();

		/// <summary>
		/// Called by OnValidate when changing values in the inspector
		/// </summary>
		protected abstract void OnInspectorChanges();
		#endregion
	}
}
