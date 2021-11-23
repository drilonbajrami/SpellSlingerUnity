using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class DisableOnStart : MonoBehaviour
    {
        void Start() => gameObject.SetActive(false);
    }
}
