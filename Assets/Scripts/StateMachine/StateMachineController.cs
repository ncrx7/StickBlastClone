using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        private IState _currentState;

        private void Start()
        {
            _currentState = new EnteringMainMenuState();

            //SwitchState(new EnteringMainMenuState()); //EXAMPLE USAGE FROM OTtER CLASS TO SWITCH STATE (other examples are on the panel open callbacks)
        }

/*         private void Update()
        {
            _currentState.OnUpdateState();
        } */

        public void SwitchState(IState newState)
        {
            _currentState.OnExitState();

            _currentState = newState;

            _currentState.OnEnterState();
        }
    }
}
