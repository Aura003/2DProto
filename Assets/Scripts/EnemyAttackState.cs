using UnityEngine;

public class EnemyAttackState : EnemyStates
{
  
   public EnemyAttackState(Enemy e) : base(e)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("ON ENTER ATTACK");
        enemy.enemyAnim.SetTrigger("attack");
        enemy.enemyAnim.SetFloat("movement", 0);
       
    }
    public override void OnUpdate()
    {

        // throw new System.NotImplementedException();
        Collider2D coll = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, enemy.PlayerLayer);
        if (coll == null)
        {
            enemy.OnChangeState(enemy.idleState);
        }
        else
        {
            enemy.enemyAnim.SetBool("forceAttack", true);
        }

    }
    public override void OnFixedUpdate()
    {
        //base.OnFixedUpdate();
    }
    
    public override void OnExit()
    {
        Debug.Log("ON EXIT ATTACK");
        enemy.enemyAnim.SetBool("forceAttack", false);
    }
 
}
