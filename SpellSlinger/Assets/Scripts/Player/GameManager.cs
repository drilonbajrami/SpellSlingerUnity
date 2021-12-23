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

        public SpellCrafter SpellCrafter;
        public GemStones GemStones;

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
            //ScorePanel.SetActive(false);
            SpellProgress.SetActive(false);
            GemStones.ResetHP();

            if (playModeOnStart) Play();
            else GoToStart();
        }

        public void ApplyGameSettings(GameSettings gameSettings)
        {
            SpellCrafter.CraftingTimer.ChangeInterval(gameSettings.CraftingDuration);
            Spawner.GetComponent<Spawner>().ApplyGameSettings(gameSettings);
        }

        public void Play()
        {
            Player.Instance.Gestures.DisableAllGestures();
            AudioManager.Instance.Stop("Menu");
            AudioManager.Instance.Play("Battle");
            MoveToSpot("StartPosition");   
            Player.Instance.ResetStats();

            Spawner.SetActive(true);

            SpellCrafter.ResetCrafting();
            SpellCrafter.Toggle(true);
            //ScorePanel.SetActive(true);
            DamageOverlay.SetActive(true);

            // Disable all other screens
            StartScreen.SetActive(false);        
            TutorialScreen.SetActive(false);
            SettingScreen.SetActive(false);
            VictoryScreen.SetActive(false);
            GameOverScreen.SetActive(false);
            Overlay.SetActive(false);

            SpellProgress.SetActive(false);
            GemStones.ResetHP();

            Player.Instance.Gestures.Enable<CraftGesture>();
            Player.Instance.Gestures.Enable<CastGesture>(); 
        }

        public void GoToStart()
        {
            Player.Instance.Gestures.DisableAllGestures();
            AudioManager.Instance.Play("Menu");
            AudioManager.Instance.Stop("Battle");
            Player.Instance.ResetStats();
            MoveToSpot("TutorialPosition");
          
            StartScreen.SetActive(true);
            Overlay.SetActive(true);

            DamageOverlay.SetActive(false);
            //ScorePanel.SetActive(false);
            SpellProgress.SetActive(false);
            SpellCrafter.ResetCrafting();
        }

        public void GoToTutorial()
        {
            Player.Instance.Gestures.DisableAllGestures();
            MoveToSpot("TutorialPosition");
            TutorialScreen.SetActive(true);
        }

        public void GoToSettings()
        {
            Player.Instance.Gestures.DisableAllGestures();
            SettingScreen.SetActive(true);
            SpellCrafter.Toggle(false);
        }

        public void OnGameEnd()
        {
            Player.Instance.Gestures.DisableAllGestures();
            AudioManager.Instance.Stop("Battle");
            MoveToSpot("TutorialPosition");
            Overlay.SetActive(true);

            //ScorePanel.SetActive(false);
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