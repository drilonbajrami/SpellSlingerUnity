using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSlinger
{
    public class AttackDeath : MonoBehaviour
    {
        private Animator mAnimator;
        // Start is called before the first frame update
        void Start()
        {
            mAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        if(Input.GetKeyDown(KeyCode.O))
            {
                mAnimator.SetTrigger("TrAttack");
            }
        
            if(Input.GetKeyDown(KeyCode.T))
            {
                mAnimator.SetTrigger("TrHit");
            }
        }
    }
}
