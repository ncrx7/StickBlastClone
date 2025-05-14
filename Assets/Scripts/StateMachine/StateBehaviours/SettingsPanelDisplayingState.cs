using UnityEngine;

namespace StateMachine
{
    public class SettingsPanelDisplayingState : IState
    {
        public void OnEnterState()
        {
            Debug.Log("Entering settings panel -> from state machine behaviour");
        }

        public void OnExitState()
        {
            Debug.Log("Exiting settings panel -> from state machine behaviour");
        }

        public void OnUpdateState()
        {
            Debug.Log("Updating settings panel -> from state machine behaviour");
        }
    }
}