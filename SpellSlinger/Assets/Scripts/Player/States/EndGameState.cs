using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class EndGameState : State
    {
        public EndGameState(Player player) : base(player) { }

        #region Inherited Methods
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
