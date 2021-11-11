using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Spell Type", menuName = "SpellType", order = 1)]
    public class SpellType : ScriptableObject
    {
        [SerializeField] private ElementalProperties properties;
        public ElementalProperties Properties { get { return properties; } }
	}
}