using System;
using UnityEngine;

namespace SpellSlinger
{
	public class GestureCaster : MonoBehaviour
	{
		public bool withTrackers = false;

		#region Private Fields
		// Casting Signs
		private const string CRAFT_POSE_HAND_L = "CASTL";
		private const string CRAFT_POSE_HAND_R = "CASTR";
		private const string CAST_SPELL_POSE = "CAST";
		private const string HELP_POSE = "HELP";
		private const string SWIPE_START_POSE = "SWIPE_START";
		private const string SWIPE_END_POSE = "SWIPE_END";
	
		[SerializeField] private HandEngine_Client leftHand;
		[SerializeField] private HandEngine_Client rightHand;
		[SerializeField] private Transform hmdTransform;

		// Caching hand poses
		private string _lHandCurrentPose;
		private string _rHandCurrentPose;
		private string _rHandCurrentSwipePose;
		private string _rHandCurrentHelpPose;
		private string _handsCurrentPoses;

		private bool rHandAboutToCastSpell = false;

		private float _rHandLastYAxisPosition;

		// Timers
		private Timer _lHandTimer;
		private Timer _rHandTimer;
		private Timer _craftPoseTimer;
		private Timer _swipePoseTimer;
		private Timer _helpPoseTimer;

		[Tooltip("The time span in seconds for a pose to remain stable in order to be captured.\n" +
				 "It is recommended to keep this time span at 0.5 seconds.")]
		[Range(0.1f, 2.0f)] [SerializeField] private float poseTimeSpan;

		[Range(0.1f, 0.3f)] [SerializeField] private float swipePoseTimeSpan = 0.2f;

		[Tooltip("Distance threshold between two hands, used for checking if hands are close enough during some specific spells/gestures.\n" +
				 "Optimal distance threshold is around 0.25f units")]
		[Range(0.2f, 0.5f)] [SerializeField] private float handDistanceThreshold = 0.25f;

		[Tooltip("Distance threshold between the right hand and the head (HMD Device).\n" +
				 "Used to check for the CAST pose.")]
		[Range(0.2f, 0.5f)] [SerializeField] private float rightHandHMDMidPointThreshold = 0.4f;
		// Average retracted arm distance = 0.15 - 0.25
		// Average extended arm distance = 0.45 - 0.65
		#endregion

		#region Event Handlers
		public static EventHandler<char> LetterPoseEvent;
		public static EventHandler CraftPoseEvent;
		public static EventHandler CastPoseEvent;
		public static EventHandler SwipePoseEvent;
		public static EventHandler<bool> HelpPoseEvent;
		#endregion

		#region Event Raisers
		private void OnLetterPose() => LetterPoseEvent?.Invoke(this, char.Parse(_lHandCurrentPose));
		private void OnCraftPose() => CraftPoseEvent?.Invoke(this, EventArgs.Empty);
		private void OnCastPose() => CastPoseEvent?.Invoke(this, EventArgs.Empty);
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
			if (leftHand.poseActive && !_lHandTimer.Running) {
				_lHandTimer.ResetTimer();
				_lHandTimer.Activate();
			}

			if (rightHand.poseActive && !_rHandTimer.Running) {
				_rHandTimer.ResetTimer();
				_rHandTimer.Activate();
			}

			if (rightHand.poseActive && !_swipePoseTimer.Running) {
				_swipePoseTimer.ResetTimer();
				_swipePoseTimer.Activate();
			}

			if (rightHand.poseActive && !_helpPoseTimer.Running) {
				_helpPoseTimer.ResetTimer();
				_helpPoseTimer.Activate();
			}

			if (leftHand.poseActive && rightHand.poseActive && !_craftPoseTimer.Running) {
				_craftPoseTimer.ResetTimer();
				_craftPoseTimer.Activate();
			}

			_lHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_rHandTimer.UpdateTimer(Time.fixedDeltaTime);
			_craftPoseTimer.UpdateTimer(Time.fixedDeltaTime);
			_swipePoseTimer.UpdateTimer(Time.fixedDeltaTime);
			_helpPoseTimer.UpdateTimer(Time.fixedDeltaTime);
		}

		private void OnValidate()
		{
			if (_lHandTimer != null && _rHandTimer != null && _craftPoseTimer != null)
			{
				_lHandTimer.ChangeInterval(poseTimeSpan);
				_rHandTimer.ChangeInterval(poseTimeSpan);
				_craftPoseTimer.ChangeInterval(poseTimeSpan);
				_swipePoseTimer.ChangeInterval(swipePoseTimeSpan);
				_helpPoseTimer.ChangeInterval(0.3f);
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
			_lHandTimer.TimerFinish += LeftHandTimerFinish;
			_lHandTimer.TimerReset += LeftHandTimerReset;

			_rHandTimer = new Timer(poseTimeSpan);
			_rHandTimer.TimerFinish += RightHandTimerFinish;
			_rHandTimer.TimerReset += RightHandTimerReset;

			_craftPoseTimer = new Timer(poseTimeSpan);
			_craftPoseTimer.TimerFinish += CraftPoseTimerFinish;
			_craftPoseTimer.TimerReset += CraftPoseTimerReset;

			_swipePoseTimer = new Timer(swipePoseTimeSpan);
			_swipePoseTimer.TimerFinish += SwipePoseTimerFinish;
			_swipePoseTimer.TimerReset += SwipePoseTimerReset;

			_helpPoseTimer = new Timer(0.3f);
			_helpPoseTimer.TimerFinish += HelpPoseTimerFinish;
			_helpPoseTimer.TimerReset += HelpPoseTimerReset;
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

		#region Event Subscriber Methods
		/// <summary>
		/// Timer Finish subscriber method for left hand.
		/// </summary>
		private void LeftHandTimerFinish(object source, EventArgs e)
		{
			if (_lHandCurrentPose == leftHand.poseName && leftHand.poseActive)
				if (_lHandCurrentPose == CRAFT_POSE_HAND_L) return;
				else if (_lHandCurrentPose.Length <= 1) OnLetterPose(); // Raises LeftHandPoseEvent
		}

		/// <summary>
		/// Timer Reset subscriber method for left hand.
		/// </summary>
		private void LeftHandTimerReset(object source, EventArgs e)
		{
			if (leftHand.poseActive) _lHandCurrentPose = leftHand.poseName;
			else _lHandCurrentPose = string.Empty;
		}

		/// <summary>
		/// Timer Finish subscriber method for right hand.
		/// </summary>
		private void RightHandTimerFinish(object source, EventArgs e)
		{
			//if (_rHandCurrentPose == rightHand.poseName && rightHand.poseActive)
			//	if (_rHandCurrentPose == CAST_SPELL_POSE && rHandAboutToCastSpell && GetDistanceFromHMDToRH() > rightHandHMDMidPointThreshold)
			//		OnCastPose();

			if (_rHandCurrentPose == rightHand.poseName && rightHand.poseActive)
			{
				if (_rHandCurrentPose == CAST_SPELL_POSE && rHandAboutToCastSpell)
				{
					if (withTrackers)
					{
						if (GetDistanceFromHMDToRH() > rightHandHMDMidPointThreshold)
							OnCastPose();
					}
					else OnCastPose();
				}
			}
			else if (_rHandCurrentPose == SWIPE_START_POSE && rightHand.poseName == SWIPE_END_POSE && rightHand.poseActive)
			{
				OnSwipePose();
			}
		}

		/// <summary>
		/// Timer Reset subscriber method for right hand.
		/// </summary>
		private void RightHandTimerReset(object source, EventArgs e)
		{
			//if (rightHand.poseActive) {
			//	_rHandCurrentPose = rightHand.poseName;
			//	if (_rHandCurrentPose == CAST_SPELL_POSE && GetDistanceFromHMDToRH() < rightHandHMDMidPointThreshold)
			//		rHandAboutToCastSpell = true;
			//	else
			//		rHandAboutToCastSpell = false;
			//}
			//else
			//	_rHandCurrentPose = string.Empty;

			if (rightHand.poseActive) {
				_rHandCurrentPose = rightHand.poseName;
				_rHandLastYAxisPosition = rightHand.transform.position.y;
				if (_rHandCurrentPose == CAST_SPELL_POSE) {
					if (!withTrackers) rHandAboutToCastSpell = true;
					else if (GetDistanceFromHMDToRH() < rightHandHMDMidPointThreshold) rHandAboutToCastSpell = true;
					else rHandAboutToCastSpell = false;
				}
			}
			else _rHandCurrentPose = string.Empty;
		}

		

		private void HelpPoseTimerFinish(object sender, EventArgs e)
		{
			if (_rHandCurrentHelpPose == rightHand.poseName && rightHand.poseActive)
			{
				if (rightHand.transform.position.y - _rHandLastYAxisPosition > 0.5f)
					OnHelpPose(true);
				else if (_rHandLastYAxisPosition - rightHand.transform.position.y > 0.5f)
					OnHelpPose(false);
			}
		}

		private void HelpPoseTimerReset(object sender, EventArgs e)
		{
			if (rightHand.poseActive)
			{
				_rHandCurrentHelpPose = rightHand.poseName;
				_rHandLastYAxisPosition = rightHand.transform.position.y;
			}
			else _rHandCurrentPose = string.Empty;
		}

		/// <summary>
		/// On Timer Finish subscriber method for both hands.
		/// </summary>
		private void CraftPoseTimerFinish(object sender, EventArgs e)
		{
			//float distance = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
			//if (_handsCurrentPoses == CRAFT_POSE_HAND_L + CRAFT_POSE_HAND_R && distance < handDistanceThreshold)
			//	OnCraftPose(); // Raises CraftPoseEvent

			float distance = Vector3.Distance(leftHand.gameObject.transform.position, rightHand.gameObject.transform.position);
			if (_handsCurrentPoses == CRAFT_POSE_HAND_L + CRAFT_POSE_HAND_R) {
				if (withTrackers && distance < handDistanceThreshold) OnCraftPose();
				else OnCraftPose();
			}
		}

		/// <summary>
		/// On Timer Reset subscriber method for both hands.
		/// </summary>
		private void CraftPoseTimerReset(object sender, EventArgs e)
		{
			if (leftHand.poseActive && rightHand.poseActive) _handsCurrentPoses = GetBothHandPoses();
			else _handsCurrentPoses = string.Empty;
		}

		/// <summary>
		/// On Timer Finish subscriber method for right hand swipe pose
		/// </summary>
		private void SwipePoseTimerFinish(object source, EventArgs e)
		{
			if (_rHandCurrentSwipePose == SWIPE_START_POSE && rightHand.poseName == SWIPE_END_POSE && rightHand.poseActive)
				OnSwipePose();
		}

		/// <summary>
		/// On Timer Reset subscriber method for right hand swipe pose
		/// </summary>
		private void SwipePoseTimerReset(object source, EventArgs e)
		{
			if (rightHand.poseActive)
				_rHandCurrentSwipePose = rightHand.poseName;
			else
				_rHandCurrentSwipePose = string.Empty;
		}
		#endregion
	}
}