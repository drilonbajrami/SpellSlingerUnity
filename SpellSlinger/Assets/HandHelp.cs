using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
	public class HandHelp : MonoBehaviour
	{
		private List<GameObject> panels = new List<GameObject>();
		private int childCount = 0;
		private int currentPanelIndex = 0;

		void Start()
		{
			childCount = gameObject.transform.childCount;

			for (int i = 0; i < childCount; i++)
			{
				GameObject panel = gameObject.transform.GetChild(i).gameObject;
				panels.Add(panel);
				panel.gameObject.SetActive(false);
			}

			currentPanelIndex = 0;
			panels[currentPanelIndex].SetActive(true);
			SwipeGesture.PoseEvent += SwipePose;
		}

		private void OnEnable()
		{
			SwipeGesture.PoseEvent += SwipePose;
		}

		private void OnDisable()
		{
			SwipeGesture.PoseEvent -= SwipePose;
		}

		private void SwipePose(object source, SwipeDirections directions)
		{
			if (directions.defaultDirection == SwipeDirection.RIGHT) {
				if (directions.currentDirection == SwipeDirection.RIGHT) Next();
				else Previous();
			} 
			else {
				if (directions.currentDirection == SwipeDirection.RIGHT) Previous();
				else Next();
			}
		}

		private void Next()
		{
			panels[currentPanelIndex].gameObject.SetActive(false);
			currentPanelIndex++;
			if (currentPanelIndex >= childCount)
				currentPanelIndex = 0;
			panels[currentPanelIndex].gameObject.SetActive(true);
		}

		private void Previous()
		{
			panels[currentPanelIndex].gameObject.SetActive(false);
			currentPanelIndex--;
			if (currentPanelIndex <= -1)
				currentPanelIndex = childCount - 1;
			panels[currentPanelIndex].gameObject.SetActive(true);
		}
	}
}
