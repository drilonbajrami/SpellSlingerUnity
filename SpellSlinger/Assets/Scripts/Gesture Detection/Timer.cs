using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Timer
    {
        private bool running;
        public bool Running => running;

        private float timerInterval;
        private float elapsedTime;

        public event EventHandler TimerStart;
        public event EventHandler TimerEnd;
        
        /// <summary>
        /// Timer class for managing timed events.
        /// </summary>
        public Timer(float seconds)
        {
            running = false;
            timerInterval = seconds;
            elapsedTime = 0.0f;
        }

        /// <summary>
        /// Increment the timer by given amount in seconds.
        /// </summary>
        public void UpdateTimer(float seconds)
        {
            if (!running) return;

            elapsedTime += seconds;
            if (elapsedTime >= timerInterval) End();
        }

        /// <summary>
        /// Reset everything about the timer and start.
        /// Invokes the TimerStart event.
        /// </summary>
        public void Start()
        {
            elapsedTime = 0.0f;
            Continue();
            TimerStart?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Ends the timer and invokes the TimerEnd event.
        /// </summary>
        private void End()
        {
            Pause();
            TimerEnd?.Invoke(this, EventArgs.Empty);           
        }

        /// <summary>
        /// Stops the timer and resets it.
        /// </summary>
        public void Stop()
        {
            elapsedTime = 0.0f;
            Pause();
        }

        public void Continue() => running = true;
        public void Pause() => running = false;

        public void ChangeInterval(float seconds) => timerInterval = seconds;
        public float GetCompletionInPercent() => elapsedTime * 100.0f / timerInterval;
    }
}