using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
    public class HandHelp : MonoBehaviour
    {
        private List<Transform> panels = new List<Transform>();
        private int childCount = 0;
        private int currentPanelIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            childCount = gameObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                panels.Add(gameObject.transform.GetChild(i));
            }

            GestureCaster.SwipePoseEvent += SwipePose;
        }

		private void OnEnable()
		{
            GestureCaster.SwipePoseEvent += SwipePose;
        }

		private void OnDisable()
		{
            GestureCaster.SwipePoseEvent -= SwipePose;
        }

		private void SwipePose(object source, EventArgs e)
        {
            if (currentPanelIndex < childCount - 1)
            {
                currentPanelIndex++;
                UpdateHelpPanel(currentPanelIndex);
            }
            else
            {
                currentPanelIndex = 0;
                UpdateHelpPanel(currentPanelIndex);
            }
        }

        private void UpdateHelpPanel(int index)
        {
            for (int i = 0; i < childCount; i++)
            {
                if (i != index)
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                else
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
