using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public Gestures Gestures;
        public State _state;

        [Space(5)]
        [Header("Gloves ON/OFF")]
        [SerializeField] private bool noGloves;
        public static bool NO_GLOVES;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);  
        }

        private void Start()
		{
            _state = new StartScreenState(this);
		}

        public void ChangeState(State state)
        {
            _state.OnExit();
            _state = state;
        }

        public void OnValidate()
        {
            NO_GLOVES = noGloves;
        }
    }
}
