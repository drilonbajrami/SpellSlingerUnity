using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class SettingsUISelector : MonoBehaviour
    {
        public GameObject selectOverlay;
        public int difficultyIndex;

        public void SelectSetting()
        {
            selectOverlay.SetActive(true);
            //GameManager.Instance.Settings.SetSettings(difficultyIndex);
        }

        public void DeselectSetting()
        {
            selectOverlay.SetActive(false);
        }
    }
}
