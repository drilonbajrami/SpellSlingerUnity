using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Game Setting", menuName = "Game Settings", order = 2)]
    public class GameSettingScriptableObject : ScriptableObject
    {
        [Range(5.0f, 30.0f)]
        public float craftingDuration;
    }

    public class GameSetting
    {
        public float craftingDuration;

        public void SetSetting(GameSettingScriptableObject gameSetting)
        {
            craftingDuration = gameSetting.craftingDuration;
        }
    }
}
