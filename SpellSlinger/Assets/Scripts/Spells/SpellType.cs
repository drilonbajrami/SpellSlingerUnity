using UnityEngine;

namespace SpellSlinger
{
    /// <summary>
    /// Scriptable object for storing spell type data.
    /// </summary>
    [CreateAssetMenu(fileName = "Spell Type", menuName = "SpellType", order = 1)]
    public class SpellType : ScriptableObject
    {
        [SerializeField] private Element _element;
        public Element Element => _element;

        [SerializeField] private Material _material;
        public Material Material => _material;

        [SerializeField] private GameObject _effect;
        public GameObject Effect => _effect;

        public string GetElementName() => Element.ToString();
        public char GetElementLetterByIndex(int index) => char.Parse(Element.ToString().Substring(index, 1));
        public int GetElementTypeNameLength() => Element.ToString().Length;
    }
}