using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpellSlinger
{
    public enum TutorialStepIndex
    {
        CRAFTGESTURE = 0,
        HELPGESTURE = 1,
        SWIPEGESTURE = 2,
        SPELLBAR = 3,
        SPELLSELECT = 4,
        HELPREMINDER = 5,
        CASTGESTURE = 6,
        RESTARTORCONTINUE = 7
    }

    public class Tutorial : MonoBehaviour
    {
        public static Sprite tutorialBackground;

        public List<GameObject> tutorialSteps;

        int counter = 0;

        #region UNITY Methods
        void Awake()
        {
            tutorialBackground = Resources.Load<Sprite>("UI/TutorialBackdrop");      
        }

        private void Start()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                tutorialSteps.Add(gameObject.transform.GetChild(i).gameObject);
                tutorialSteps[i].SetActive(false);
            }

            Activate(false);
        }

        private void OnEnable()
        {
            counter = 0;
            tutorialSteps[counter].SetActive(true);
        }

        private void OnDisable()
        {
            foreach(GameObject step in tutorialSteps)
                step.SetActive(false);
        }
        #endregion

        public void Activate(bool e) => gameObject.SetActive(e);

        public void NextStep()
        {
            tutorialSteps[counter].SetActive(false);
            counter++;
            if (counter == tutorialSteps.Count)
                counter = 0;

            tutorialSteps[counter].SetActive(true);
        }

        public void JumpToStep(TutorialStepIndex index)
        {
            tutorialSteps[counter].SetActive(false);
            counter = (int)index;
            tutorialSteps[counter].SetActive(true);
        }

        public bool IsInStep(TutorialStepIndex index) => counter == (int)index;
    }
}
