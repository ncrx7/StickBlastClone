using UnityEngine;

namespace StateMachine
{
    public class HomePanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            Debug.Log("Entering Home panel -> from state machine behaviour");
        }

        public void OnExitState()
        {
            Debug.Log("Exiting Home panel -> from state machine behaviour");
        }

        public void OnUpdateState()
        {
            Debug.Log("Updating Home panel -> from state machine behaviour");
        }
    }
}