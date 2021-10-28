using System;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		#region Private Fields
		// Casting Signs
		private const string START_POSE_HAND_L = "CASTL";
		private const string START_POSE_HAND_R = "CASTR";
		private const string CAST_SPELL_POSE = "CAST";

		// HandEngine clients for reading pose data
		[SerializeField] private HandEngine_Client leftHand;
		[SerializeField] private HandEngine_Client rightHand;
		[SerializeField] private Transform hmdTransform;

		// Caching hand poses
		private string _lHandCurrentPose;
		private string _rHandCurrentPose;
		private string _handsCurrentPoses;

		private bool rHandAboutToCastSpell = false;

		// Timers
		private Timer _lHandTimer;
		private Timer _rHandTimer;
		private Timer _startCastTimer;

		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] private float poseTimeSpan;

		[Tooltip("Distance threshold between two hands, used for checking if hands are close enough during some specific spells/gestures.\n" +
				 "Optimal distance threshold is around 0.25f units")]
		[Range(0.2f, 0.5f)] [SerializeField] private float handDistanceThreshold = 0.25f;

		[Tooltip("Distance threshold between the right hand and the head (HMD Device).\n" +
				 "Used to check for the CAST pose.")]
		[Range(0.2f, 0.5f)] [SerializeField] private float rightHandHMDMidPointThreshold = 0.4f;
		// Average retracted arm distance = 0.15 - 0.25
		// Average extended arm distance = 0.45 - 0.65

		#endregion

		#region Events

		// Event handlers
		public static event EventHandler<string> LHandPose;
		public static event EventHandler<string> RHandPose;
		public static event EventHandler StartSpellingPose;
		public static event EventHandler CastSpellPose;

		// Event raisers
		protected virtual void OnLeftHandPose() => LHandPose?.Invoke(this, _lHandCurrentPose);
		protected virtual void OnRightHandPose() => RHandPose?.Invoke(this, _rHandCurrentPose);
		protected virtual void OnStartSpellingPose() => StartSpellingPose?.Invoke(this, EventArgs.Empty);
		protected virtual void OnCastSpellPose() => CastSpellPose?.Invoke(this, EventArgs.Empty);

		#endregion

		#region UNITY Methods

		private void Start()
		{
			SetupTimers();
		}

		private void FixedUpdate()
		{
			if (leftHand.poseActive && !_lHandTimer.Running) {
				_lHandTimer.ResetTimer();
				_lHandTimer.Activate();
			}

			if (rightHand.poseActive && !_rHandTimer.Running) {
				_rHandTimer.ResetTimer();
				_rHandTimer.Activate();
			}

			if (leftHand.poseActive && rightHand.poseActive && !_startCastTimer.Running) {
				_startCastTimer.ResetTimer();
				_startCastTimer.Activate();
			}

			_lHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_rHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_startCastTimer.UpdateTimer(Time.fixedDeltaTime);
		}

		private void OnValidate()
		{
			if (_lHandTimer != null && _rHandTimer != null && _startCastTimer != null)
			{
				_lHandTimer.ChangeInterval(poseTimeSpan);
				_rHandTimer.ChangeInterval(poseTimeSpan);
				_startCastTimer.ChangeInterval(poseTimeSpan);
			}
		}

		#endregion

		#region Private Methods

		private void SetupTimers()
		{
			_lHandTimer = new Timer(poseTimeSpan);
			_lHandTimer.TimerEnd += OnLeftHandTimerEnd;
			_lHandTimer.TimerReset += OnLeftHandTimerReset;

			_rHandTimer = new Timer(poseTimeSpan);
			_rHandTimer.TimerEnd += OnRightHandTimerEnd;
			_rHandTimer.TimerReset += OnRightHandTimerReset;

			_startCastTimer = new Timer(poseTimeSpan);
			_startCastTimer.TimerEnd += OnStartSpellingTimerEnd;
			_startCastTimer.TimerReset += OnStartSpellingTimerReset;
		}

		/// <summary>
		/// Returns a string containing both hands poses
		/// </summary>
		/// <returns></returns>
		private string GetBothHandPoses() { return leftHand.poseName + rightHand.poseName; }

		private float GetDistanceHMDToRightHand()
		{
			return Vector2.Distance(new Vector2(hmdTransform.position.x, hmdTransform.position.z), new Vector2(rightHand.transform.position.x, rightHand.transform.position.z));
		}

		#endregion

		#region OnEvent Methods

		private void OnLeftHandTimerEnd(object source, EventArgs e)
		{
			if (_lHandCurrentPose == leftHand.poseName && leftHand.poseActive)
				if (_lHandCurrentPose == START_POSE_HAND_L) return;
				else if (_lHandCurrentPose.Length <= 1) OnLeftHandPose();
		}

		private void OnLeftHandTimerReset(object source, EventArgs e)
		{
			if (leftHand.poseActive) _lHandCurrentPose = leftHand.poseName;
			else _lHandCurrentPose = string.Empty;
		}

		private void OnRightHandTimerEnd(object source, EventArgs e)
		{
			if (_rHandCurrentPose == rightHand.poseName && rightHand.poseActive)
				if (_rHandCurrentPose == CAST_SPELL_POSE && rHandAboutToCastSpell && GetDistanceHMDToRightHand() > rightHandHMDMidPointThreshold)
					OnCastSpellPose();
				else if (_rHandCurrentPose.Length <= 1) OnRightHandPose();
		}

		private void OnRightHandTimerReset(object source, EventArgs e)
		{
			if (rightHand.poseActive)
			{
				_rHandCurrentPose = rightHand.poseName;
				if (_rHandCurrentPose == CAST_SPELL_POSE && GetDistanceHMDToRightHand() < rightHandHMDMidPointThreshold)
					rHandAboutToCastSpell = true;
				else
					rHandAboutToCastSpell = false;
			}
			else
				_rHandCurrentPose = string.Empty;
		}

			private void OnStartSpellingTimerEnd(object sender, EventArgs e)
		{
			float distance = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
			if (_handsCurrentPoses == START_POSE_HAND_L + START_POSE_HAND_R && distance < handDistanceThreshold)
				OnStartSpellingPose();
		}

		private void OnStartSpellingTimerReset(object sender, EventArgs e)
		{
			if (leftHand.poseActive && rightHand.poseActive) _handsCurrentPoses = GetBothHandPoses();
			else _handsCurrentPoses = string.Empty;
		}

		#endregion
	}
}