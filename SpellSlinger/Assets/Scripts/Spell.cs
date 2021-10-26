using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    [System.Serializable]
    public class Spell : MonoBehaviour
    {
        [SerializeField] private Element _type;
        [SerializeField] private Color _color;

        public Element GetElementType() => _type;
        public string GetElementTypeName() => _type.ToString();
        public char GetElementStartingLetter() => char.Parse(_type.ToString().Substring(1));
    }
}
