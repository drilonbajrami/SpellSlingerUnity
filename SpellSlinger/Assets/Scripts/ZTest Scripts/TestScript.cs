using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpellSlinger
{
    public class TestScript : MonoBehaviour
    {

        private void Awake()
        {
            print("Awake Called");    
        }

        private void Start()
        {
            print("Start Called");
        }

        private void OnEnable()
        {
            print("OnEnable Called");
        }

        private void OnDisable()
        {
            print("OnDisable Called");
        }
    }
}
