using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
	public class HandHelp : MonoBehaviour
	{
		//[SerializeField] Transform hand;

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

		//private void Update()
		//{
		//	transform.parent.transform.position = new Vector3(hand.position.x + 0.03f, hand.position.y + 0.15f, hand.position.z + 0.08f);
		//}

		private void OnEnable()
		{
			SwipeGesture.PoseEvent += SwipePose;
		}

		private void OnDisable()
		{
			SwipeGesture.PoseEvent -= SwipePose;
		}

		private void SwipePose(object source, EventArgs e)
		{
			Next();
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
