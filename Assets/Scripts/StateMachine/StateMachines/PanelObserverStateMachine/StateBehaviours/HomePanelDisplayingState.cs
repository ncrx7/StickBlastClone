using UnityEngine;

namespace StateMachine
{
    public class HomePanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            #if UNITY_EDITOR
            Debug.Log("Entering Home panel -> from state machine behaviour");
            #endif
        }

        public void OnExitState()
        {
            #if UNITY_EDITOR
            Debug.Log("Exiting Home panel -> from state machine behaviour");
            #endif
        }

        public void Tick()
        {
            #if UNITY_EDITOR
            Debug.Log("Updating Home panel -> from state machine behaviour");
            #endif
        }
    }
}