using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSlinger
{
    public class Tutorial : MonoBehaviour
    {
        public static Sprite tutorialBackground;
        // Start is called before the first frame update
        void Awake()
        {
            tutorialBackground = Resources.Load<Sprite>("UI/TutorialBackdrop");
        }
    }
}
