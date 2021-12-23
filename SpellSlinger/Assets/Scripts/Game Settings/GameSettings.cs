using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Game Setting", menuName = "Game Settings", order = 2)]
    public class GameSettings : ScriptableObject
    {
        [Range(5.0f, 30.0f)] public float CraftingDuration;
        [Range(1.0f, 30.0f)] public float EnemySpawnRate;
        [Range(1, 25)] public int EnemiesToSpawn;
    }
}