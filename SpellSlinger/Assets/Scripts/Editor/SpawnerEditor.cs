using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpellSlinger
{
	[CustomEditor(typeof(Spawner))]
	public class SpawnerEditor : Editor
	{
		Spawner spawner;
		string turnRecordingOnOff = "Start Recording Spawn Positions";
		string recordingOn = "Stop Recording Spawn Positions";
		string recordingOff = "Start Recording Spawn Positions";

		private GameObject lastCreatedSpawnPoint;

		Transform objectPreview;  //the transform to move
		Transform objectTouched = null; //the reference of the last object hit

		public void OnSceneGUI()
		{
			spawner = (Spawner)target;

			//if (spawner.recordingSpawnPoints)
			//{

			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				Debug.Log("Mouse Clicked");

			//Event e = Event.current;

			//	if (e.type == EventType.MouseDown && e.button == 0)
			//	{
			//		Debug.Log("Click...");
			//		Debug.Log("Recording is on...");
			//		e.Use();
			//		Vector3 screenPosition = Event.current.mousePosition;
			//		screenPosition.y = Camera.current.pixelHeight - screenPosition.y;
			//		Ray ray = Camera.current.ScreenPointToRay(screenPosition);
			//		RaycastHit hit;
			//		//// use a different Physics.Raycast() override if necessary
			//		if (Physics.Raycast(ray, out hit))
			//		{
			//			Debug.Log(hit.point);
			//		//	Debug.Log("Creating spawn point");
			//		//	GameObject spawnPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//		//	spawnPoint.transform.position = hit.point;
			//		//	spawner.AddSpawnPoint(spawnPoint);
			//		//	Debug.Log("Spawn point Created");
			//		//	// do stuff here using hit.point
			//		//	// tell the event system you consumed the click
			//			//Event.current.Use();
			//		}

			//		//Vector3 pointsPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

			//		////Todo create object here at pointsPos
			//		//lastCreatedSpawnPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//		//lastCreatedSpawnPoint.transform.position = pointsPos;
			//		//spawner.AddSpawnPoint(lastCreatedSpawnPoint);
			//		//// Avoid the current event being propagated
			//		//// I'm not sure which of both works better here
			//		//Event.current.Use();
			//		//Event.current = null;

			//		//// Keep the created object in focus
			//		//Selection.activeGameObject = lastCreatedSpawnPoint;
					

			//		//Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			//		//RaycastHit hitInfo;

			//		//if (Physics.Raycast(worldRay, out hitInfo, Mathf.Infinity))
			//		//{
			//		//	if (hitInfo.collider.gameObject != null)
			//		//	{
			//		//		objectTouched = hitInfo.collider.gameObject.transform;
			//		//		objectPreview.position = hitInfo.point;
			//		//		objectPreview.rotation = Quaternion.Euler(hitInfo.normal);
			//		//		Debug.Log(hitInfo.point);
			//		//	}
			//		//}


			//	}
			//}
		}

		public override void OnInspectorGUI()
		{
			spawner = (Spawner)target;
			DrawDefaultInspector();

			if (GUILayout.Button(turnRecordingOnOff))
			{
				if (!spawner.recordingSpawnPoints)
					spawner.TurnRecordingOn();
				else
					spawner.TurnRecordingOff();

				if (spawner.recordingSpawnPoints) turnRecordingOnOff = recordingOn;
				else turnRecordingOnOff = recordingOff;
			}

			if (GUILayout.Button("Remove last Spawn point")) spawner.RemoveLastPoint();
			if (GUILayout.Button("Clear all Spawn points")) spawner.ClearAllPoints();
		}
	}
}
