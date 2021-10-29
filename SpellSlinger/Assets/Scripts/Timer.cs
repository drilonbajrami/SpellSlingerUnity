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
        public bool Running { get { return running; } }

        /// <summary>
        /// Event Handler for ending of the timer
        /// </summary>
        public event EventHandler TimerFinish;
        /// <summary>
        /// Event Handler for reseting of the timer
        /// </summary>
        public event EventHandler TimerReset;

        /// <summary>
        /// Timer class for managing timed events
        /// </summary>
        /// <param name="seconds">Timer interval in seconds</param>
        public Timer(float seconds)
        {
            timerInterval = seconds;
            elapsedTime = 0.0f;
        }

        /// <summary>
        /// Increment the timer by give amount in seconds
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateTimer(float seconds)
        {
            if (!running) return;

            elapsedTime += seconds;

            if (elapsedTime >= timerInterval)
            {
                TimerFinish?.Invoke(this, EventArgs.Empty);
                ResetTimer();
            }
        }

        /// <summary>
        /// Change timer interval
        /// </summary>
        /// <param name="seconds"></param>
        public void ChangeInterval(float seconds) => timerInterval = seconds;

        /// <summary>
        /// Reset everything about the timer.
        /// Invokes the TimerReset event handler
        /// </summary>
        public void ResetTimer()
        {
            running = false;
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

        /// <summary>
        /// Returns the progress of the timer in terms of percentage
        /// </summary>
        /// <returns></returns>
        public float GetProgressPercentage() => elapsedTime * 100.0f / timerInterval;
    }
}