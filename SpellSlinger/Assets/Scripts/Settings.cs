using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Settings : MonoBehaviour
    {
        public List<GameSettings> gameSettings = new List<GameSettings>();

        public GameSettings selectedSettings = null;

        public void SetSettings(GameSettings settings)
        {
            
        }
    }
}
