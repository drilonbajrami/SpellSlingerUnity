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

		public override void OnInspectorGUI()
		{
			spawner = (Spawner)target;
			DrawDefaultInspector();

			if (GUILayout.Button("Add spawn point")) Selection.activeGameObject = spawner.AddSpawnPoint();
			if (GUILayout.Button("Remove selected")) spawner.RemoveSelectedPoint(Selection.activeGameObject);
			if (GUILayout.Button("Clear all")) spawner.ClearAllPoints();
		}
	}
}
