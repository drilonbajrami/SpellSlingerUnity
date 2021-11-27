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
        [SerializeField] private bool _isEnabled = false;
		protected bool IsEnabled => _isEnabled;

		// Tracker availability
		private bool _areTrackersOn;
		protected bool AreTrackersOn => _areTrackersOn;

		[Header("Pose timer duration")]
		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] protected float _poseTimeSpan = 0.5f;

		// Pose Timer
		protected Timer _timer;

        #region UNITY Methods
        private void Start()
		{
			// Setup the timer and subscribe to timer events
			_timer = new Timer(_poseTimeSpan);
			_timer.TimerStart += PoseStart;
			_timer.TimerEnd += PoseEnd;
		}		

		private void FixedUpdate()
		{
			// If gesture is disabled then don't update
			if (!_isEnabled) return;

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
		public void ChangePoseTimeSpan(float seconds) => _poseTimeSpan = seconds;

		/// <summary>
		/// Enables the gesture
		/// </summary>
		public void Enable()
		{
			_isEnabled = true;
			Debug.Log($"Enabled {gameObject.name}");
		}
        
		/// <summary>
		/// Disables the gesture
		/// </summary>
		public void Disable()
		{
			_isEnabled = false;
			Debug.Log($"Disabled {gameObject.name}");
		}
        
		/// <summary>
		/// Enable and Disable tracking
		/// </summary>
		/// <param name="state"></param>
		public void ToggleTracking(bool state) => _areTrackersOn = state;
		#endregion

		#region Abstract Methods
		/// <summary>
		/// Returns the state of the hand poses
		/// </summary>
		abstract protected bool PoseIsActive { get; }

		/// <summary>
		/// On Timer Start event subscriber method
		/// </summary>
		protected virtual void PoseStart(object sender, EventArgs e) { }

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
		protected virtual void OnInspectorChanges() { }
		#endregion
	}
}
