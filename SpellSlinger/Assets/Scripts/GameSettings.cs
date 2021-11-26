using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Game Setting", menuName = "Game Settings", order = 2)]
    public class GameSettings : ScriptableObject
    {
        public float craftingTimerDuration;
    }
}
