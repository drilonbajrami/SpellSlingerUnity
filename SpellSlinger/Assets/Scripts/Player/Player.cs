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

        [Space(5)]
        [Header("Gloves ON/OFF")]
        [SerializeField] private bool noGloves;
        public static bool NO_GLOVES;

        [HideInInspector] public ScoreManager ScoreManager;

        bool playModeOnStart = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            ScoreManager = GetComponent<ScoreManager>();
        }

        private void Update()
        {
            if (playModeOnStart)
            {
                Gestures.Enable<CraftGesture>();
                Gestures.Enable<CastGesture>();
                playModeOnStart = false;
            }
        }

        public void OnValidate()
        {
            NO_GLOVES = noGloves;
        }
    }
}
