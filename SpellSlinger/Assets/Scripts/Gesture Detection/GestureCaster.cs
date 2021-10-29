using System;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		#region Private Fields
		// Casting Signs
		private const string CRAFT_POSE_HAND_L = "CASTL";
		private const string CRAFT_POSE_HAND_R = "CASTR";
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
		private Timer _spellingPoseTimer;

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

		#region Event Handlers & Raisers

		/// <summary>
		/// Event Handler for left hand poses.
		/// </summary>
		public static event EventHandler<string> LeftHandPose;
		protected virtual void OnLeftHandPose() => LeftHandPose?.Invoke(this, _lHandCurrentPose);
		/// <summary>
		/// Event Handler for right hand poses.
		/// </summary>
		public static event EventHandler<string> RightHandPose;
		protected virtual void OnRightHandPose() => RightHandPose?.Invoke(this, _rHandCurrentPose);
		/// <summary>
		/// Event Handler for starting spelling pose.
		/// </summary>
		public static event EventHandler CraftPose;
		protected virtual void OnCraftPose() => CraftPose?.Invoke(this, EventArgs.Empty);
		/// <summary>
		/// Event Handler for casting spell pose.
		/// </summary>
		public static event EventHandler CastPose;
		protected virtual void OnCastPose() => CastPose?.Invoke(this, EventArgs.Empty);

		#endregion

		#region UNITY Methods

		private void Start()
		{
			SetupTimers(poseTimeSpan);
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

			if (leftHand.poseActive && rightHand.poseActive && !_spellingPoseTimer.Running) {
				_spellingPoseTimer.ResetTimer();
				_spellingPoseTimer.Activate();
			}

			_lHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_rHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_spellingPoseTimer.UpdateTimer(Time.fixedDeltaTime);
		}

		private void OnValidate()
		{
			if (_lHandTimer != null && _rHandTimer != null && _spellingPoseTimer != null)
			{
				_lHandTimer.ChangeInterval(poseTimeSpan);
				_rHandTimer.ChangeInterval(poseTimeSpan);
				_spellingPoseTimer.ChangeInterval(poseTimeSpan);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Set up separate and common timers for both hands.
		/// </summary>
		private void SetupTimers(float poseTimeSpan)
		{
			_lHandTimer = new Timer(poseTimeSpan);
			_lHandTimer.TimerFinish += OnLeftHandTimerFinish;
			_lHandTimer.TimerReset += OnLeftHandTimerReset;

			_rHandTimer = new Timer(poseTimeSpan);
			_rHandTimer.TimerFinish += OnRightHandTimerFinish;
			_rHandTimer.TimerReset += OnRightHandTimerReset;

			_spellingPoseTimer = new Timer(poseTimeSpan);
			_spellingPoseTimer.TimerFinish += OnStartSpellingTimerFinish;
			_spellingPoseTimer.TimerReset += OnStartSpellingTimerReset;
		}

		/// <summary>
		/// Returns a string containing both hands poses.
		/// </summary>
		/// <returns></returns>
		private string GetBothHandPoses() => leftHand.poseName + rightHand.poseName; 

		/// <summary>
		/// Returns the distance between the HMD and Right Hand on the XZ Plane.
		/// </summary>
		/// <returns></returns>
		private float GetDistanceFromHMDToRH()
		{
			return Vector2.Distance(new Vector2(hmdTransform.position.x, hmdTransform.position.z), new Vector2(rightHand.transform.position.x, rightHand.transform.position.z));
		}

		#endregion

		#region OnEvent Methods

		/// <summary>
		/// On Timer Finish subscriber method for left hand.
		/// </summary>
		private void OnLeftHandTimerFinish(object source, EventArgs e)
		{
			if (_lHandCurrentPose == leftHand.poseName && leftHand.poseActive)
				if (_lHandCurrentPose == CRAFT_POSE_HAND_L) return;
				else if (_lHandCurrentPose.Length <= 1) OnLeftHandPose();
		}

		/// <summary>
		/// On Timer Reset subscriber method for left hand.
		/// </summary>
		private void OnLeftHandTimerReset(object source, EventArgs e)
		{
			if (leftHand.poseActive) _lHandCurrentPose = leftHand.poseName;
			else _lHandCurrentPose = string.Empty;
		}

		/// <summary>
		/// On Timer Finish subscriber method for right hand.
		/// </summary>
		private void OnRightHandTimerFinish(object source, EventArgs e)
		{
			if (_rHandCurrentPose == rightHand.poseName && rightHand.poseActive)
				if (_rHandCurrentPose == CAST_SPELL_POSE && rHandAboutToCastSpell && GetDistanceFromHMDToRH() > rightHandHMDMidPointThreshold)
					OnCastPose();
				else if (_rHandCurrentPose.Length <= 1) OnRightHandPose();
		}

		/// <summary>
		/// On Timer Reset subscriber method for right hand.
		/// </summary>
		private void OnRightHandTimerReset(object source, EventArgs e)
		{
			if (rightHand.poseActive)
			{
				_rHandCurrentPose = rightHand.poseName;
				if (_rHandCurrentPose == CAST_SPELL_POSE && GetDistanceFromHMDToRH() < rightHandHMDMidPointThreshold)
					rHandAboutToCastSpell = true;
				else
					rHandAboutToCastSpell = false;
			}
			else
				_rHandCurrentPose = string.Empty;
		}

		/// <summary>
		/// On Timer Finish subscriber method for both hands.
		/// </summary>
		private void OnStartSpellingTimerFinish(object sender, EventArgs e)
		{
			float distance = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
			if (_handsCurrentPoses == CRAFT_POSE_HAND_L + CRAFT_POSE_HAND_R && distance < handDistanceThreshold)
				OnCraftPose();
		}

		/// <summary>
		/// On Timer Reset subscriber method for both hands.
		/// </summary>
		private void OnStartSpellingTimerReset(object sender, EventArgs e)
		{
			if (leftHand.poseActive && rightHand.poseActive) _handsCurrentPoses = GetBothHandPoses();
			else _handsCurrentPoses = string.Empty;
		}

		#endregion
	}
}