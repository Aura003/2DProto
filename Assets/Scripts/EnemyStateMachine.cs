using UnityEngine;

public class EnemyStateMachine
{
    private EnemyStates currentState;

    public void ChangeState(EnemyStates newState)
    {
        if (currentState == newState)
            return;

        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();

    }
    public void Update()
    {
        currentState?.OnUpdate();
    }
    public void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }
}
