using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Timer
    {
        private bool running;
        public bool Running { get { return running; } }

        private float timerInterval;
        private float elapsedTime;

        /// <summary>
        /// Event Handler for reseting of the timer
        /// </summary>
        public event EventHandler TimerStart;

        /// <summary>
        /// Event Handler for ending of the timer
        /// </summary>
        public event EventHandler TimerEnd;
        
        /// <summary>
        /// Timer class for managing timed events
        /// </summary>
        /// <param name="seconds">Timer interval in seconds</param>
        public Timer(float seconds)
        {
            running = false;
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
                End();
        }

        /// <summary>
        /// Ends the timer
        /// </summary>
        private void End()
        {
            Pause();
            TimerEnd?.Invoke(this, EventArgs.Empty);           
        }

        /// <summary>
        /// Reset everything about the timer and start.
        /// Invokes the TimerStart event handler
        /// </summary>
        public void Start()
        {
            elapsedTime = 0.0f;
            Continue(); 
            TimerStart?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Continues the timer
        /// </summary>
        public void Continue() => running = true;

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Pause() => running = false;

        /// <summary>
        /// Change timer interval
        /// </summary>
        public void ChangeInterval(float seconds) => timerInterval = seconds;

        /// <summary>
        /// Returns the progress of the timer in terms of percentage
        /// </summary>
        /// <returns></returns>
        public float GetProgressPercentage() => elapsedTime * 100.0f / timerInterval;
    }
}