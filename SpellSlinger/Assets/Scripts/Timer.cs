using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Timer
    {
        private float timerInterval;
        private float elapsedTime;

        private bool running = true;

        public event EventHandler TimerEnd;
        public event EventHandler TimerReset;

        /// <summary>
        /// Timer class for managing timed events
        /// </summary>
        /// <param name="seconds">Timer interval in seconds</param>
        public Timer(float seconds)
        {
            timerInterval = seconds;
            elapsedTime = 0.0f;
            running = true;
        }   

        /// <summary>
        /// Decrements the timer by give amount
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateTimer(float seconds)
        {
            if (!running)
                return;

            elapsedTime += seconds;

            if (elapsedTime >= timerInterval)
            {
                TimerEnd?.Invoke(this, EventArgs.Empty);
                ResetTimer();
            }
        }

        /// <summary>
        /// Change timer interval
        /// </summary>
        /// <param name="seconds"></param>
        public void ChangeInterval(float seconds) => timerInterval = seconds;

        public void ResetTimer()
        {
            elapsedTime = 0.0f;
            TimerReset?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Activates the timer
        /// </summary>
        public void Activate() => running = true;

        /// <summary>
        /// Deactivates the timer
        /// </summary>
        public void Deactivate() => running = false;
    }
}
