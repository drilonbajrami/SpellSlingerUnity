using UnityEngine;

namespace SpellSlinger
{
    public class SettingsUISelector : MonoBehaviour
    {
        public GameObject selectOverlay;

        public void Select()
        {
            selectOverlay.SetActive(true);
        }

        public void Deselect()
        {
            selectOverlay.SetActive(false);
        }
    }
}