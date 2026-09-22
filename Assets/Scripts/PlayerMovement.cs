using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("GoundCheck")]
    public Transform GroundCheckPoint;
    public float Radius;
    private int groundLayer { get { return 1 << LayerMask.NameToLayer("Ground"); } }
    private PlayerInput playerInput;
    private Animator myAnim;
    private Rigidbody2D rb2D;

    private Vector2 movementVector;
    private float jumpForce = 6f; 
    [SerializeField] private float moveSpeed;

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
        playerInput.Player.Attack.started += OnAttack;
        playerInput.Player.Jump.performed += OnJumpPerformed;
        playerInput.Player.Roll.performed += OnRollPerformed;
        playerInput.Player.Block.performed += OnBlockPerformed;
    }

    

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMovementRead;
        playerInput.Player.Move.canceled -= OnMovementStopRead;
        playerInput.Player.Attack.started -= OnAttack;
        playerInput.Player.Jump.performed -= OnJumpPerformed;
        playerInput.Player.Roll.performed -= OnRollPerformed;
        playerInput.Player.Block.performed -= OnBlockPerformed;
    }
    private void OnDestroy()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    private void OnAttack(InputAction.CallbackContext context)
    {
        myAnim.SetTrigger("attack");
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!IsGrounded())
            return;

        Jump();
    }
    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        if (movementVector.sqrMagnitude < 0.01)
            return;

        myAnim.SetTrigger("roll");
    }
    private void OnBlockPerformed(InputAction.CallbackContext context)
    {
        //Logic for block, need to be standing or walking to block, block disables the movement releasing the block enables the movement again
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
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
    void Jump()
    {
        Debug.Log("Performing Jump!");
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        myAnim.SetTrigger("jump");
    }
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheckPoint.position, Radius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPoint.position, Radius);
    }
}
