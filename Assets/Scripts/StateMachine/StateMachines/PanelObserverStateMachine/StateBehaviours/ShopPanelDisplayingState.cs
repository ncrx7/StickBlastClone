using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class ShopPanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            #if UNITY_EDITOR
            Debug.Log("Entering shop panel -> from state machine behaviour");
            #endif
        }

        public void OnExitState()
        {
            #if UNITY_EDITOR
            Debug.Log("Exiting shop panel -> from state machine behaviour");
            #endif
        }

        public void Tick()
        {
            #if UNITY_EDITOR
            Debug.Log("Updating shop panel -> from state machine behaviour");
            #endif
        }
    }
}
