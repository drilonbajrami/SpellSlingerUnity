using System;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		#region CAST SIGNS
		// Casting Signs
		private const string START_POSE_HAND_L = "CASTL";
		private const string START_POSE_HAND_R = "CASTR";
		private const string CAST_SPELL_POSE = "CAST";
		#endregion

		// HandEngine clients for reading pose data
		[SerializeField] private HandEngine_Client _leftHand;
		[SerializeField] private HandEngine_Client _rightHand;

		// Caching hand poses
		private string _lHandCurrentPose;
		private string _rHandCurrentPose;
		private string _handsCurrentPoses;

		private Timer _lHandPoseTimer;
		private Timer _rHandPoseTimer;
		private Timer _startCastPoseTimer;

		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] private float poseTimeSpan;

		[Tooltip("Distance threshold between two hands, used for checking if hands are close enough during some specific spells/gestures.\n" +
				 "Optimal distance threshold is around 0.25f units")]
		[Range(0.2f, 0.5f)] [SerializeField] private float handDistanceThreshold = 0.25f;

		// Events
		public static event EventHandler<string> LHandPose;
		public static event EventHandler<string> RHandPose;
		public static event EventHandler StartCastHandPose;
		public static event EventHandler CastSpellHandPose;

		private void Start()
		{
			SetupTimers();
		}

		private void SetupTimers()
		{
			_lHandPoseTimer = new Timer(poseTimeSpan);
			_lHandPoseTimer.TimerEnd += OnLeftHandTimerEnd;
			_lHandPoseTimer.TimerReset += OnLeftHandTimerReset;

			_rHandPoseTimer = new Timer(poseTimeSpan);
			_rHandPoseTimer.TimerEnd += OnRightHandTimerEnd;
			_rHandPoseTimer.TimerReset += OnRightHandTimerReset;

			_startCastPoseTimer = new Timer(poseTimeSpan);
			_startCastPoseTimer.TimerEnd += OnStartCastTimerEnd;
			_startCastPoseTimer.TimerReset += OnStartCastTimerReset;
		}

		private void FixedUpdate()
		{
			_lHandPoseTimer.UpdateTimer(Time.fixedDeltaTime);
			_rHandPoseTimer.UpdateTimer(Time.fixedDeltaTime);
			_startCastPoseTimer.UpdateTimer(Time.fixedDeltaTime);
		}

		#region Left Hand

		private void OnLeftHandTimerEnd(object source, EventArgs e)
		{
			if (_lHandCurrentPose == _leftHand.poseName && _leftHand.poseActive) OnLeftHandPose();
		}

		private void OnLeftHandTimerReset(object source, EventArgs e)
		{
			if (_leftHand.poseActive) _lHandCurrentPose = _leftHand.poseName;
			else _lHandCurrentPose = string.Empty;
		}

		#endregion

		#region Right Hand

		private void OnRightHandTimerEnd(object source, EventArgs e)
		{
			if (_rHandCurrentPose == _rightHand.poseName && _rightHand.poseActive)
				if (_rHandCurrentPose == CAST_SPELL_POSE) OnCastSpellPose();
				else OnRightHandPose();
		}

		private void OnRightHandTimerReset(object source, EventArgs e)
		{
			if (_rightHand.poseActive) _rHandCurrentPose = _rightHand.poseName;
			else _rHandCurrentPose = string.Empty;
		}

		#endregion

		#region Both Hands

		private void OnStartCastTimerReset(object sender, EventArgs e)
		{
			float distance = Vector3.Distance(_leftHand.gameObject.transform.position, _rightHand.gameObject.transform.position);
			if (_handsCurrentPoses == START_POSE_HAND_L + START_POSE_HAND_R && distance < handDistanceThreshold)
				OnStartCastPose();
		}

		private void OnStartCastTimerEnd(object sender, EventArgs e)
		{
			if (_leftHand.poseActive && _rightHand.poseActive) _handsCurrentPoses = GetBothHandPoses();
			else _handsCurrentPoses = string.Empty;
		}

		private string GetBothHandPoses() { return _leftHand.poseName + _rightHand.poseName; }

		#endregion

		// Event raisers
		protected virtual void OnLeftHandPose() => LHandPose?.Invoke(this, _lHandCurrentPose);
		protected virtual void OnRightHandPose() => RHandPose?.Invoke(this, _rHandCurrentPose);
		protected virtual void OnStartCastPose() => StartCastHandPose?.Invoke(this, EventArgs.Empty);
        protected virtual void OnCastSpellPose() => CastSpellHandPose?.Invoke(this, EventArgs.Empty);
	}
}