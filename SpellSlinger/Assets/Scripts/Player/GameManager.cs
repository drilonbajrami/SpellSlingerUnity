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

        public GameObject Spawner;
        public GameSetting gameSettings = new GameSetting();

        public SpellCrafter SpellCrafter;

        [Space(10)]
        [Header("UI Screens")]
        public GameObject StartScreen;
        public GameObject TutorialScreen;
        public GameObject SettingsSreen;
        public GameObject VictoryScreen;
        public GameObject GameOverScreen;
        public GameObject Overlay;
        public GameObject DamageOverlay;
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
            if(playModeOnStart)
            {
                StraightToPlay();
            }
            else
            {
                ResetGame();
            }
        }

        public void ApplyGameSettings(GameSettingScriptableObject gameSettingsSO)
        {
            gameSettings.SetSetting(gameSettingsSO);
            SpellCrafter.CraftingTimer.ChangeInterval(gameSettings.craftingDuration);
        }

        public void ResetGame()
        {
            Player.Instance.ResetStats();
            StartScreen.SetActive(true);
            Overlay.SetActive(true);
            DamageOverlay.SetActive(true);
            TutorialScreen.SetActive(false);
            SettingsSreen.SetActive(false);
            VictoryScreen.SetActive(false);
            GameOverScreen.SetActive(false);
            ScorePanel.SetActive(false);
            SpellProgress.SetActive(false);
            SpellCrafter.ResetCrafting();
        }

        public void StraightToPlay()
        {
            GameObject startingPosition = GameObject.Find("StartPosition");
            Player.Instance.transform.position = startingPosition.transform.position;
            Player.Instance.transform.rotation = startingPosition.transform.rotation;
            Player.Instance.ResetStats();
            StartScreen.SetActive(false);
            Overlay.SetActive(false);
            DamageOverlay?.SetActive(true);
            TutorialScreen.SetActive(false);
            SettingsSreen.SetActive(false);
            VictoryScreen.SetActive(false);
            GameOverScreen.SetActive(false);
            ScorePanel.SetActive(true);
            SpellProgress.SetActive(false);
            SpellCrafter.ResetCrafting();
            Player.Instance.Gestures.Enable<CraftGesture>();
            Player.Instance.Gestures.Enable<CastGesture>();
        }

        /// <summary>
        /// List all available tracking devices and print out the serial and model numbers
        /// </summary>
		void ListDevices()
        {
            for (int i = 0; i < SteamVR.connected.Length; ++i)
            {
                ETrackedPropertyError error = new ETrackedPropertyError();
                StringBuilder sb = new StringBuilder();
                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var SerialNumber = sb.ToString();

                OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
                var ModelNumber = sb.ToString();
                if (SerialNumber.Length > 0 || ModelNumber.Length > 0)
                    Debug.Log("Device " + i.ToString() + " = " + SerialNumber + " | " + ModelNumber);
            }
        }
    }
}