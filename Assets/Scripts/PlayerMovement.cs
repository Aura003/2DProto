using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator myAnim;
    private Rigidbody2D rb2D;

    private Vector2 movementVector;
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
    }

    

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMovementRead;
        playerInput.Player.Move.canceled -= OnMovementStopRead;
        playerInput.Player.Attack.started -= OnAttack;
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
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        rb2D.linearVelocity = movementVector * moveSpeed;
    }
    void InvertSprite(Vector2 movement)
    {
        if (movement.x < 0)
            this.transform.localScale = new Vector3(-1, 1, 1);
        else
            this.transform.localScale = Vector2.one;
    }
}
