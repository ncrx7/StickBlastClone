using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class EnteringMainMenuState : IState
    {
        public void OnEnterState()
        {
            Debug.Log("Entering the game 'enter game state' -> From State Machine");
        }

        public void OnExitState()
        {
            Debug.Log("Exiting the 'enter game state' -> From State Machine");
        }

        public void OnUpdateState()
        {
            Debug.Log("Updating the 'enter game state' -> From State Machine");
        }
    }
}
