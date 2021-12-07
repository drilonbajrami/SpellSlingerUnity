using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Game Setting", menuName = "Game Settings", order = 2)]
    public class GameSettingSO : ScriptableObject
    {
        [Range(5.0f, 30.0f)] public float craftingDuration;
    }

    public class GameSetting {
        private float _craftingDuration;
        public float CraftingDuration => _craftingDuration;

        public void SetSetting(GameSettingSO gameSetting) 
                => _craftingDuration = gameSetting.craftingDuration;
    }
}
