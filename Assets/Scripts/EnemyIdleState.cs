using UnityEngine;

public class EnemyIdleState : EnemyStates
{
    public EnemyIdleState(Enemy e) : base(e)
    {

    }
    public override void OnEnter()
    {
        Debug.Log("ON ENTER IDLE STATE");
        //enemy.enemyAnim.SetTrigger("attack");
    }
    public override void OnUpdate()
    {
        Collider2D coll = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, enemy.PlayerLayer);
        if (coll != null)
            enemy.OnChangeState(enemy.attackState);
        else
            enemy.OnChangeState(enemy.walkState);
    }
    public override void OnExit()
    {
        Debug.Log("ON EXIT IDLE STATE");
    }
   
}
