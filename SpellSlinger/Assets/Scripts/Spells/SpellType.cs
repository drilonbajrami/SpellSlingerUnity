using UnityEngine;

namespace SpellSlinger
{
    /// <summary>
    /// Scriptable object for storing spell type data.
    /// </summary>
    [CreateAssetMenu(fileName = "Spell Type", menuName = "SpellType", order = 1)]
    public class SpellType : ScriptableObject
    {
        [SerializeField] private ElementalProperties properties;
        public ElementalProperties Properties => properties;
	}
}