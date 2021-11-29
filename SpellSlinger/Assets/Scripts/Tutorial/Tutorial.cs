using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public enum StepIndex
    {
        CRAFT   = 1,
        HELP    = 2,
        SWIPE   = 3,
        SELECT  = 4,
        REMIND  = 5,
        CAST    = 6,
        LAST    = 7
    }

    public class Tutorial : MonoBehaviour
    {
        // Cache the tutorial background sprite
        [HideInInspector] public Sprite tutorialBackground;

        public List<GameObject> tutorialSteps;
        int counter = 0;

        #region UNITY Methods
        void Awake()
        {
            tutorialBackground = Resources.Load<Sprite>("UI/TutorialBackdrop");

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                tutorialSteps.Add(gameObject.transform.GetChild(i).gameObject);
                tutorialSteps[i].SetActive(false);
            }
        }

        private void OnEnable()
        {
            counter = 0;

            if(tutorialSteps.Count != 0)
                tutorialSteps[counter].SetActive(true);  
        }

        private void OnDisable()
        {
            foreach(GameObject step in tutorialSteps)
                step.SetActive(false);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Goes to the next tutorial step
        /// </summary>
        public void NextStep()
        {
            tutorialSteps[counter].SetActive(false);
            counter++;
            if (counter == tutorialSteps.Count)
                counter = 0;

            tutorialSteps[counter].SetActive(true);
        }

        /// <summary>
        /// Jump to any tutorial step by index
        /// </summary>
        /// <param name="index"></param>
        public void JumpToStep(StepIndex index)
        {
            tutorialSteps[counter].SetActive(false);
            counter = (int)index - 1;
            tutorialSteps[counter].SetActive(true);
        }

        /// <summary>
        /// Returns true if current tutorial step has the given step index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsInStep(StepIndex index) => counter == (int)index - 1;
        #endregion
    }
}