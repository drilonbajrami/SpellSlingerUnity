using UnityEngine;

namespace SpellSlinger
{
    /// <summary>
    /// Scriptable object for storing enemy type data.
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Type", menuName = "EnemyType", order = 1)]
    public class EnemyType : ScriptableObject
    {
        [SerializeField] private ElementalProperties properties;
        public ElementalProperties Properties => properties;
    }
}