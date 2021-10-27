using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [CreateAssetMenu(fileName = "Spell Type", menuName = "SpellType", order = 1)]
    public class SpellType : ScriptableObject
    {
        public Element type;
        public Color color;

        public Element GetElementType() => type;
        public string GetElementTypeName() => type.ToString();
        public char GetElementLetterByIndex(int index) => char.Parse(type.ToString().Substring(index, 1));
        public int GetElementTypeNameLength() => type.ToString().Length;
    }
}
