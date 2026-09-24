using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHp = 100;
    private float currentHp;
    [HideInInspector]public Animator enemyAnim;
    private EnemyStateMachine enemyFSM;

    private EnemyIdleState idleState;
    private void Awake()
    {
        enemyFSM = new EnemyStateMachine();
        idleState = new EnemyIdleState(this);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnChangeState(idleState);
        enemyAnim = this.GetComponent<Animator>();
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        enemyFSM.Update();
    }
    private void FixedUpdate()
    {
        enemyFSM.FixedUpdate();
    }
    private void OnChangeState(EnemyStates newState)
    {
        enemyFSM.ChangeState(newState);
    }
    public void TakeDamage(int Amount, bool isCritical)
    {
        currentHp -= Amount;
        GameManager.Instance.ShowNumbers(Amount,this.transform.position +Vector3.up, isCritical);
        enemyAnim.SetTrigger("hurt");
        if (currentHp <= 0)
        {
            Dead();
        }

    }
    void Dead()
    {
        enemyAnim.SetTrigger("dead");
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
