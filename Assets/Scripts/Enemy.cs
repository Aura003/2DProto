using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private int maxHp = 100;
    [SerializeField]private int EXPToGive;
    public Transform AttackPoint;
    public GameObject ProjectilePrefab;
    private float currentHp;
    [HideInInspector]public Animator enemyAnim;
    private EnemyStateMachine enemyFSM;

    public EnemyIdleState idleState;
    public EnemyWalkState walkState;
    public EnemyAttackState attackState;

    [Header("FSM STUFF")]
    public float detectionRange;
    public int PlayerLayer {  get { return 1 << LayerMask.NameToLayer("Player"); } }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);
    }
    private void Awake()
    {
        enemyFSM = new EnemyStateMachine();
        idleState = new EnemyIdleState(this);
        walkState = new EnemyWalkState(this);
        attackState = new EnemyAttackState(this);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnChangeState(idleState);
        enemyAnim = this.GetComponent<Animator>();
        currentHp = maxHp;
    }

    // Update is called once per frame
    public void Update()
    {
        enemyFSM.Update();
    }
    public void FixedUpdate()
    {
        enemyFSM.FixedUpdate();
    }
    public void OnChangeState(EnemyStates newState)
    {
        enemyFSM.ChangeState(newState);
    }
    public void OnExitState()
    {
        enemyFSM.ExitState();
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
    public void AnimationEventAttack()
    {
        Vector2 facingDir = this.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
      GameObject proj = Instantiate(ProjectilePrefab,AttackPoint.position,AttackPoint.rotation);
        proj.GetComponent<ProjectileLogic>().SetDirection(facingDir);
        proj.transform.localScale = new Vector3(proj.transform.localScale.x * facingDir.x, proj.transform.localScale.y, proj.transform.localScale.z);
    }
    void Dead()
    {
        enemyAnim.SetTrigger("dead");
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
