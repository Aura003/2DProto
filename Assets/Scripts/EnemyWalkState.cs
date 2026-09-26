using UnityEngine;

public class EnemyWalkState : EnemyStates
{
    public EnemyWalkState(Enemy e):base(e)
    {

    }
    private Vector3 startingPos;
    private Vector3 targetRight, targetLeft;
    private float walkDistance = 1.5f;
    private float walkSpeed = 1.5f;
    bool isMovingRight = false;
    bool foundEnemy = false;
    public override void OnEnter()
    {
        Debug.Log("ON ENTER WALK STATE");
        startingPos = enemy.transform.position;
        targetRight = new Vector3(startingPos.x + (walkDistance), enemy.transform.position.y, enemy.transform.position.z);
        targetLeft = new Vector3(startingPos.x - (walkDistance), enemy.transform.position.y, enemy.transform.position.z);
        isMovingRight = false;
        UpdateFacing();
    }
    public override void OnUpdate()
    {
        // throw new System.NotImplementedException();
        //Collider2D coll = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, enemy.PlayerLayer);
        //if (coll != null)
        //    enemy.OnChangeState(enemy.attackState);
    }
    public override void OnFixedUpdate()
    {
        //base.OnFixedUpdate();
        float direction = isMovingRight ? 1 : -1;
        if (!foundEnemy)
        {
            enemy.EnemyRbdy2D.linearVelocity = new Vector2(direction * walkSpeed, enemy.EnemyRbdy2D.linearVelocity.y);
            enemy.enemyAnim.SetFloat("movement", Mathf.Abs(direction));
            if(isMovingRight && enemy.transform.position.x >= startingPos.x + walkDistance)
            {
                isMovingRight = false;
                UpdateFacing();
            }
            else if(!isMovingRight && enemy.transform.position.x <= startingPos.x - walkDistance)
            {
                isMovingRight = true;
                UpdateFacing();
            }

        }

        Collider2D coll = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, enemy.PlayerLayer);
        if (coll != null)
        {
            foundEnemy = true;
            enemy.OnChangeState(enemy.attackState);
        }
        
    }
    public override void OnExit()
    {
        Debug.Log("ON EXIT WALK STATE");
        foundEnemy = false;
        enemy.EnemyRbdy2D.linearVelocity = new Vector2(0, enemy.EnemyRbdy2D.linearVelocity.y);

    }
    void UpdateFacing()
    {
        Vector3 scale = enemy.transform.localScale;
        scale.x = isMovingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        enemy.transform.localScale = scale;
    }
}
