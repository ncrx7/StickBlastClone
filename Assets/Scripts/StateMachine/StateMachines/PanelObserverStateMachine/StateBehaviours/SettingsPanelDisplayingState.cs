using UnityEngine;

namespace StateMachine
{
    public class SettingsPanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            #if UNITY_EDITOR
            Debug.Log("Entering settings panel -> from state machine behaviour");
            #endif
        }

        public void OnExitState()
        {
            #if UNITY_EDITOR
            Debug.Log("Exiting settings panel -> from state machine behaviour");
            #endif
        }

        public void Tick()
        {
            #if UNITY_EDITOR
            Debug.Log("Updating settings panel -> from state machine behaviour");
            #endif
        }
    }
}