using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public abstract class State
    {
        protected State() => OnStateStart();

        public abstract void OnStateStart();

        public abstract void OnStateEnd();
    }
}