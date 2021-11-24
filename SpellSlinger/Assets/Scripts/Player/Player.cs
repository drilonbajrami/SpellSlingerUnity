using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public Gestures Gestures;
        public State _state;

        [Space(5)]
        [Header("Gloves ON/OFF")]
        [SerializeField] private bool noGloves;
        public static bool NO_GLOVES;

        public GameObject OverlayCanvas;
        
        [Space(10)]
        public GameObject spellPrefab;
        private GameObject currentSpell;
        public GameObject hand;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);  
        }

        private void Start()
		{
            SpellCrafter.CraftSpell += OnCreateSpell;
            CastGesture.PoseForm += OnCastSpell;
            _state = new StartScreenState(this);
		}

		private void Update()
		{
            if (currentSpell != null)
                currentSpell.transform.position = hand.transform.position;
		}

        public void ChangeState(State state)
        {
            _state.OnExit();
            _state = state;
        }
        
		public void OnCreateSpell(object source, SpellType spellType)
        {
            if (spellType != null)
            {
                currentSpell = Instantiate(spellPrefab);
                currentSpell.GetComponent<Spell>().SetType(spellType);
            }
        }

        public void OnCastSpell(object source, EventArgs e)
        {
            if (currentSpell != null)
                CastSpell();
        }

        private void CastSpell()
        {
            currentSpell.GetComponent<Spell>().CastSpell();
            currentSpell = null;
        }

        public void OnValidate()
        {
            NO_GLOVES = noGloves;
        }
    }
}
