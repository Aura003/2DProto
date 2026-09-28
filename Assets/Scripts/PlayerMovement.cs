using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour, IDamagable
{
    public Stats PlayerStats;
    [Header("GoundCheck")]
    public Transform GroundCheckPoint;
    [Header("Attack")]
    public Transform AttackPoint1;
    public Transform AttackPoint2;
    public Transform AttackPoint3;
    public List<AttackComboStats>AttackList = new List<AttackComboStats> ();
    [Header("ATTACK COMBO")]
    public float ComboWidow = 0.5f;
    private float comboTimer;
    private int comboIndex;
    private bool hasAttackStarted = false;
    public float AttackRadius1;
    public float AttackRadius2;
    public float AttackRadius3;

    [Header("JUMPING")]
    [SerializeField]private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    private float jumpForce = 7f;

    private float coyoteTimer;
    private float jumpButterTimer;

    [Header("BLOCK")]
    private bool isBlocking = false;

    private float groundCheckRadius = 0.05f;
    private int groundLayer { get { return 1 << LayerMask.NameToLayer("Ground"); } }
    private int enemyLayer { get { return 1 << LayerMask.NameToLayer("Enemy"); } }
    private PlayerInput playerInput;
    private Animator myAnim;
    private Rigidbody2D rb2D;

    private Vector2 movementVector;
    [SerializeField] private float moveSpeed;

    public static event Action<float, float> OnHpChanged;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMovementRead;
        playerInput.Player.Move.canceled += OnMovementStopRead;
        playerInput.Player.Attack.started += OnAttackPerformed;
        playerInput.Player.Attack.canceled += OnAtatckCanceled;
        playerInput.Player.Jump.performed += OnJumpPerformed;
        playerInput.Player.Jump.canceled += OnJumpCancelled;
        playerInput.Player.Roll.performed += OnRollPerformed;
        playerInput.Player.Block.performed += OnBlockPerformed;
        playerInput.Player.Block.canceled += OnBlockCanceled;
    }

   

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMovementRead;
        playerInput.Player.Move.canceled -= OnMovementStopRead;
        playerInput.Player.Attack.started -= OnAttackPerformed;
        playerInput.Player.Attack.canceled -= OnAtatckCanceled;
        playerInput.Player.Jump.performed -= OnJumpPerformed;
        playerInput.Player.Jump.canceled -= OnJumpCancelled;
        playerInput.Player.Roll.performed -= OnRollPerformed;
        playerInput.Player.Block.performed -= OnBlockPerformed;
        playerInput.Player.Block.canceled -= OnBlockCanceled;
    }
    private void OnDestroy()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        comboIndex = 0;
    }
    public void OnMovementRead(InputAction.CallbackContext context)
    {
        movementVector= context.ReadValue<Vector2>();
        InvertSprite(movementVector);
        myAnim.SetFloat("movement", Mathf.Abs(movementVector.x));
    }
    private void OnMovementStopRead(InputAction.CallbackContext context)
    {
        movementVector = Vector2.zero;
        myAnim.SetFloat("movement", Mathf.Abs(movementVector.x));
    }
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (comboIndex == 0)
        {
            StartAttackLogic(comboIndex, AttackList[comboIndex].AttackPoint, AttackList[comboIndex].Radius);
            hasAttackStarted = true;
            comboTimer = ComboWidow;
        }
        if (comboIndex!=0 && comboIndex < 3 && comboTimer > 0f)
        {
            StartAttackLogic(comboIndex, AttackList[comboIndex].AttackPoint, AttackList[comboIndex].Radius);
        }
    }
    private void OnAtatckCanceled(InputAction.CallbackContext context)
    {
        if (comboIndex >= 3) 
        {
            ResetCombo();
        }
        if (comboIndex < 3)
            comboIndex++;
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpButterTimer = jumpBufferTime;
    }
    private void OnJumpCancelled(InputAction.CallbackContext context)
    {
        //Jump cancelled
    }
    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        if (movementVector.sqrMagnitude < 0.01)
            return;

        myAnim.SetTrigger("roll");
    }
    private void OnBlockPerformed(InputAction.CallbackContext context)
    {
        movementVector = Vector3.zero;
        isBlocking = true; 
        myAnim.SetBool("block", isBlocking);
    }
    private void OnBlockCanceled(InputAction.CallbackContext context)
    {
        //isBlocking = false;
        //myAnim.SetBool("block", isBlocking);
    }
    void Update()
    {
        if (hasAttackStarted)
            comboTimer -= Time.deltaTime;
        if (comboTimer <= 0)
            ResetCombo();

        UpdateJumpTimer();
    }
    void FixedUpdate()
    {
        if (isBlocking)
            return;
        HandleMovement();
        HandleJump();
    }
    
    void HandleMovement()
    {
        rb2D.linearVelocity = new Vector2(movementVector.x * moveSpeed, rb2D.linearVelocity.y);
    }
    void InvertSprite(Vector2 movement)
    {
        if (movement.x < 0)
            this.transform.localScale = new Vector3(-1, 1, 1);
        else
            this.transform.localScale = Vector2.one;
    }
    void UpdateJumpTimer()
    {
        if (IsGrounded())
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpButterTimer > 0)
        {
            jumpButterTimer -= Time.deltaTime;
        }
    }
    void HandleJump()
    {
        if(coyoteTimer<=0) return;
        if(jumpButterTimer<=0) return;
        Jump();
    }
    void Jump()
    {
        Debug.Log("Performing Jump!");
        coyoteTimer = 0f;
        jumpButterTimer = 0f;
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        myAnim.SetTrigger("jump");
    }
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheckPoint.position, groundCheckRadius, groundLayer);
    }
    void StartAttackLogic(int Index, Transform attackPoint, float radius)
    {
        myAnim.SetTrigger((AttackList[comboIndex].TriggerName));
        Collider2D[]colls = Physics2D.OverlapCircleAll(AttackList[Index].AttackPoint.position, AttackList[Index].Radius, enemyLayer);
        int currentDamage = PlayerStats.Damage;
        bool isCrit = PlayerStats.RollCrit();
        if (isCrit) currentDamage *= 2;
        foreach(Collider2D x in colls)
        {
            x.GetComponent<EnemyWitch>().TakeDamage(currentDamage, isCrit);
        }
    }
    void ResetCombo()
    {
        comboTimer = 0f;
        comboIndex = 0;
        hasAttackStarted = false;
    }
    public void AnimationEvent_EndBlock()
    {
        isBlocking = false;
        myAnim.SetBool("block", isBlocking);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(AttackPoint1.position, AttackRadius1);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(AttackPoint2.position, AttackRadius2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint3.position, AttackRadius3);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || PlayerStats.CurrentHp <= 0)
            return;
        GameManager.Instance.ShowNumbers(damage, this.transform.position + Vector3.up);
        PlayerStats.CurrentHp = Mathf.Max(PlayerStats.CurrentHp - damage, 0);
        OnHpChanged?.Invoke(PlayerStats.CurrentHp, PlayerStats.MaxHp);
        Debug.LogError(PlayerStats.CurrentHp);
    }
}
[System.Serializable]
public class AttackComboStats 
{
    public string TriggerName;
    public Transform AttackPoint;
    public float Radius;
}

