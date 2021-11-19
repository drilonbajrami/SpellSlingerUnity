using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public abstract class State
    {
        protected string stateName;

        protected State() { }

        public abstract void HandleState(Player player);

        public string GetStateName() => stateName;
    }
}
