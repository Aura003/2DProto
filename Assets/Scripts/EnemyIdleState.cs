using UnityEngine;

public class EnemyIdleState : EnemyStates
{
    public EnemyIdleState(Enemy e) : base(e)
    {

    }
    public override void OnEnter()
    {
        enemy.enemyAnim.SetTrigger("attack");
    }
    public override void OnUpdate()
    {
        Debug.LogError("IDLE--STATE--UPDATE");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
   
}
