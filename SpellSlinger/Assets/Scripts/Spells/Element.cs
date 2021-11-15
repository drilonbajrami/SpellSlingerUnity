using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public enum Element { FIRE, WATER, GRASS }

    /// <summary>
    /// Stores the element properties such as current type, strength and weakness type.
    /// </summary>
    [System.Serializable]
    public struct ElementalProperties
    {
        [SerializeField] private Color color;
        [SerializeField] private Material material;
        [SerializeField] private Element currentType;
        [SerializeField] private Element strengthType;
        [SerializeField] private Element weaknessType;

        public ElementalProperties(ElementalProperties pProperties)
        {
            color = pProperties.color;
            material = pProperties.material;
            currentType = pProperties.currentType;
            strengthType = pProperties.strengthType;
            weaknessType = pProperties.weaknessType;
        }

        public Color GetColor() => color;
        public Material GetMaterial() => material;
        public Element GetElementType() => currentType;
        public Element GetStrengthElementType() => strengthType;
        public Element GetWeaknessElementType() => weaknessType;

        public string GetElementTypeName() => currentType.ToString();
        public char GetElementLetterByIndex(int index) => char.Parse(currentType.ToString().Substring(index, 1));
        public int GetElementTypeNameLength() => currentType.ToString().Length;
    }
}
