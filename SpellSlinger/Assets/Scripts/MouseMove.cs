using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    /// <summary>
    /// Camera mouse controller 
    /// </summary>
    public class MouseMove : MonoBehaviour
    {
        public bool moveOnlyXAxis = true;

        public float mouseSensitivity = 800f;
        float xRotation = 0f;

        void Update() => Rotation();

        private void Rotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            
            if(!moveOnlyXAxis)
            {
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -60f, 60f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
            
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}
