using UnityEngine;

public class EnemyWalkState : EnemyStates
{
    public EnemyWalkState(Enemy e):base(e)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
