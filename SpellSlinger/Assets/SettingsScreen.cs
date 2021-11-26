using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class SettingsScreen : MonoBehaviour
    {
        public GameObject thumbsUpContinue;
        public List<SettingsUISelector> settingSelectors;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void SelectSetting(int index)
        {
            for(int i = 0; i < settingSelectors.Count; i++)
            {
                if(i == index)
                    settingSelectors[i].DeselectSetting();
                else
                    settingSelectors[i].SelectSetting();

                if(!thumbsUpContinue.activeSelf)
                    thumbsUpContinue.SetActive(true);
            }    
        }
    }
}