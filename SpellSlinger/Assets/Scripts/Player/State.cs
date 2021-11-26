using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public abstract class State
    {
        private protected Player _player;

        protected State(Player player)
        {
            _player = player;
        }

        public abstract void OnEnter();

        public abstract void OnExit();
    }
}