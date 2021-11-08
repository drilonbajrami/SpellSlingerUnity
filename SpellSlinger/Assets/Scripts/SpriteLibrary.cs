using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpellSlinger
{
    public class SpriteLibrary : MonoBehaviour
    {
        public static Dictionary<char, Sprite> Alphabet = new Dictionary<char, Sprite>();
		public static Dictionary<string, Sprite> Elements = new Dictionary<string, Sprite>();
		private char[] alpha;

		private void Awake()
		{		
			alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

			// Store all sign letter sprites
			for (int i = 0; i < alpha.Length; i++)
			{
				Sprite letter = Resources.Load<Sprite>("UI/GestureHints/Letters/" + $"{alpha[i]}");

				if (letter != null)
					Alphabet.Add(alpha[i], letter);
			}

			// Store all element background sprites
			foreach(string name in Enum.GetNames(typeof(Element)))
			{
				Sprite background = Resources.Load<Sprite>("UI/GestureHints/" + $"{name}BG");

				if (background != null)
					Elements.Add(name, background);
			}
		}
	}
}
