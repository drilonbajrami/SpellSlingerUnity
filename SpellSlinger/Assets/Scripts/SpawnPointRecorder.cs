using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
#if UNITY_EDITOR
    using UnityEditor;
    /// <summary>
    ///
    /// </summary>
    [ExecuteInEditMode]
    public class SpawnPointRecorder : MonoBehaviour
    {
        private void OnEnable()
        {
            if (!Application.isEditor)
            {
                Destroy(this);
            }
            //SceneView.duringSceneGui += OnScene;
        }

        void OnScene(SceneView scene)
        {
            Event e = Event.current;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Debug.Log("Middle Mouse was pressed");

                Vector3 mousePos = e.mousePosition;
                float ppp = EditorGUIUtility.pixelsPerPoint;
                mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
                mousePos.x *= ppp;

                Ray ray = scene.camera.ScreenPointToRay(mousePos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //Do something, ---Example---
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = hit.point;
                    Debug.Log("Instantiated at " + hit.point);
                }
                e.Use();
            }
        }
    }
#endif
}
