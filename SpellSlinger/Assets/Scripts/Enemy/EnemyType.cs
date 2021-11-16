using UnityEngine;

namespace SpellSlinger
{
    /// <summary>
    /// Scriptable object for storing enemy type data.
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Type", menuName = "EnemyType", order = 1)]
    public class EnemyType : ScriptableObject
    {
        [SerializeField] private Element _element;
        public Element Element => _element;

        [SerializeField] private Element _strength;
        public Element Strength => _strength;

        [SerializeField] private Element _weakness;
        public Element Weakness => _weakness;

        [SerializeField] private Color _color;
        public Color Color => _color;
    }
}