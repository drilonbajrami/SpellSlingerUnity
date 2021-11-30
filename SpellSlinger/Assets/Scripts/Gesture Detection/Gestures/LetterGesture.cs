using System;
using UnityEngine;

namespace SpellSlinger
{
	public class LetterGesture : Gesture
	{
        [Header("Hand")]
		[SerializeField] private HandEngine_Client hand;

		// Cache the alphabet
		private char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

		// Cache last received pose name
		private string _lastPose;

        // Pose event
        public static event EventHandler<char> PoseForm;

        private void Update()
        {
			// Keyboard mode
            if(Player.NO_GLOVES && Input.anyKeyDown && IsEnabled) {
				string currentKey = Input.inputString;
				char key;
				if (currentKey.Length > 0) {
					currentKey = currentKey.Substring(0, 1).ToUpper();
					key = char.Parse(currentKey);
					foreach (char c in alpha) {
						if (c == key) PoseForm?.Invoke(this, key);
					}
				}
            }
        }

        // Overridden Methods
        #region Inherited Methods
        protected override bool PoseIsActive => hand.poseActive;

		protected override void OnPose() => PoseForm?.Invoke(this, char.Parse(_lastPose));

		protected override void PoseStart(object sender, EventArgs e) => _lastPose = hand.poseName; 

		protected override void PoseEnd(object sender, EventArgs e)
		{
			if (PoseIsActive && _lastPose == hand.poseName && _lastPose.Length <= 1)
				OnPose();
		}
		#endregion
	}
}