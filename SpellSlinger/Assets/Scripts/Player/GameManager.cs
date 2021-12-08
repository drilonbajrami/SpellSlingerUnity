using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Valve.VR;
using System.Text;

namespace SpellSlinger
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // Enemy Spawner
        public GameObject Spawner;

        // Game Settings
        public GameSetting gameSettings = new GameSetting();

        public SpellCrafter SpellCrafter;

        [Space(10)]
        [Header("UI Screens")]
        // Screens
        public GameObject StartScreen;
        public GameObject TutorialScreen;
        public GameObject SettingScreen;
        public GameObject VictoryScreen;
        public GameObject GameOverScreen;

        // Overlays
        public GameObject Overlay;
        public GameObject DamageOverlay;

        // UI Feedback 
        public GameObject ScorePanel;
        public GameObject SpellProgress;

        [Space(10)]
        [SerializeField] private bool playModeOnStart = true;

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
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.ResetStats();
            Spawner.SetActive(false);
            StartScreen.SetActive(false);
            TutorialScreen.SetActive(false);
            SettingScreen.SetActive(false);
            VictoryScreen.SetActive(false);
            GameOverScreen.SetActive(false);
            Overlay.SetActive(false);
            DamageOverlay.SetActive(false);
            ScorePanel.SetActive(false);
            SpellProgress.SetActive(false);

            if (playModeOnStart) Play();
            else GoToStart();
        }

        public void ApplyGameSettings(GameSettingSO gameSettingsSO)
        {
            gameSettings.SetSetting(gameSettingsSO);
            SpellCrafter.CraftingTimer.ChangeInterval(gameSettings.CraftingDuration);
        }

        public void Play()
        {
            AudioManager.Instance.Stop("Menu");
            AudioManager.Instance.Play("Battle");
            MoveToSpot("StartPosition");
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.ResetStats();

            Spawner.SetActive(true);

            SpellCrafter.ResetCrafting();
            SpellCrafter.Toggle(true);
            ScorePanel.SetActive(true);
            DamageOverlay.SetActive(true);

            // Disable all other screens
            StartScreen.SetActive(false);        
            TutorialScreen.SetActive(false);
            SettingScreen.SetActive(false);
            VictoryScreen.SetActive(false);
            GameOverScreen.SetActive(false);
            Overlay.SetActive(false);

            SpellProgress.SetActive(false);
            
            Player.Instance.Gestures.Enable<CraftGesture>();
            Player.Instance.Gestures.Enable<CastGesture>(); 
        }

        public void GoToStart()
        {
            AudioManager.Instance.Play("Menu");
            AudioManager.Instance.Stop("Battle");
            Player.Instance.ResetStats();
            MoveToSpot("TutorialPosition");
            Player.Instance.Gestures.DisableAllGestures();
            StartScreen.SetActive(true);
            Overlay.SetActive(true);

            DamageOverlay.SetActive(false);
            ScorePanel.SetActive(false);
            SpellProgress.SetActive(false);
            SpellCrafter.ResetCrafting();
            Player.Instance.Gestures.Enable<ThumbsUpGesture>();
        }

        public void GoToTutorial()
        {
            MoveToSpot("TutorialPosition");
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.Gestures.Enable<CraftGesture>();
            TutorialScreen.SetActive(true);
        }

        public void GoToSettings()
        {
            Player.Instance.Gestures.DisableAllGestures();
            Player.Instance.Gestures.Enable<LetterGesture>();
            SettingScreen.SetActive(true);
            SpellCrafter.Toggle(false);
        }

        public void OnGameEnd()
        {
            Overlay.SetActive(true);

            ScorePanel.SetActive(false);
            DamageOverlay.SetActive(false);
            SpellProgress.SetActive(false);
            Spawner.SetActive(false); ;
            SpellCrafter.ResetCrafting();
        }

        public void MoveToSpot(string posName)
        {
            GameObject spot = GameObject.Find(posName);
            Player.Instance.transform.position = spot.transform.position;
            Player.Instance.transform.rotation = spot.transform.rotation;
        }
    }
}