using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Enemy Type", menuName = "EnemyType", order = 1)]
    public class EnemyType : ScriptableObject
    {
        [SerializeField] private ElementalProperties properties;
        public ElementalProperties Properties { get { return properties; } }
    }
}