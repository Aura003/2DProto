using UnityEngine;

public class EnemyAttackState : EnemyStates
{
   public EnemyAttackState(Enemy e) : base(e)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("ON ENTER ATTACK STATE");
        enemy.enemyAnim.SetTrigger("attack");
    }
    public override void OnUpdate()
    {
        
       // throw new System.NotImplementedException();
    }
    public override void OnFixedUpdate()
    {
        //base.OnFixedUpdate();
    }
    public override void OnExit()
    {
        
    }
 
}
