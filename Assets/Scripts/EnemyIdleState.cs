using UnityEngine;

public class EnemyIdleState : EnemyStates
{
    private float IdleTimer = 5f;
    public EnemyIdleState(Enemy e) : base(e)
    {

    }
    public override void OnEnter()
    {
        Debug.Log("ON ENTER IDLE STATE");
    }
    public override void OnUpdate()
    {

        Collider2D coll = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, enemy.PlayerLayer);
        if (coll != null)
            enemy.OnChangeState(enemy.attackState);
        else
        {
            IdleTimer -= Time.deltaTime;
            if(IdleTimer<=0)
                enemy.OnChangeState(enemy.walkState);
        }
    }
    public override void OnExit()
    {
        Debug.Log("ON EXIT IDLE STATE");
        IdleTimer = 5f;
    }
   
}
