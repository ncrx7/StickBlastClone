using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class ShopPanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            Debug.Log("Entering shop panel -> from state machine behaviour");
        }

        public void OnExitState()
        {
            Debug.Log("Exiting shop panel -> from state machine behaviour");
        }

        public void OnUpdateState()
        {
            Debug.Log("Updating shop panel -> from state machine behaviour");
        }
    }
}
