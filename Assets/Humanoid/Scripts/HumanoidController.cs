using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidController : MonoBehaviour
{
    public float speed = 5;
    public float rotationSpeed = 30;
    public float jumpSpeed = 10;

    private Animator animator;
    private Rigidbody rb;
    private Collider humanCollider;
    private float distanceToGround;
    private HumanoidInput input;
    private bool isMoving;
    private bool isJumping;
    private bool isFiring;
    private int isJumpingAnimatorId;
    private int isFallingAnimatorId;
    private int isFiringAnimatorId;
    private int verticalInputAnimatorId;
    private int horizontalInputAnimatorId;
    Vector2 movementInput;


    void Awake()
    {
        input = new HumanoidInput();
        input.Player.Move.started += onMove;
        input.Player.Move.performed += onMove;
        input.Player.Move.canceled += onMove;
        input.Player.Jump.started += onJump;
        input.Player.Fire.started += onFire;
        input.Player.Fire.canceled += onFire;
    }

    private void onMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (movementInput.y != 0 || movementInput.x != 0)
            isMoving = true;
        if (movementInput.y == 0 && movementInput.x == 0)
            isMoving = false;
        animator.SetFloat(verticalInputAnimatorId, movementInput.y);
        animator.SetFloat(horizontalInputAnimatorId, movementInput.x);
    }

    private void onJump(InputAction.CallbackContext ctx)
    {
        isJumping = true;
    }

    private void onFire(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            isFiring = true;
        else
            isFiring = false;
        animator.SetBool(isFiringAnimatorId, isFiring);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        humanCollider = GetComponent<Collider>();
        distanceToGround = humanCollider.bounds.extents.y;
        animator = GetComponent<Animator>();
        isJumpingAnimatorId = Animator.StringToHash("isJumping");
        verticalInputAnimatorId = Animator.StringToHash("verticalInput");
        horizontalInputAnimatorId = Animator.StringToHash("horizontalInput");
        isFallingAnimatorId = Animator.StringToHash("isFalling");
        isFiringAnimatorId = Animator.StringToHash("isFiring");
    }

    void FixedUpdate()
    {
        Vector3 oldVelocity = rb.velocity;
        Vector3 newVelocity = Vector3.zero;

        handleMovement(ref newVelocity);
        hanldeJumping(oldVelocity, ref newVelocity);

       
        rb.velocity = newVelocity;
    }

    private void handleMovement(ref Vector3 newVelocity)
    {
        if (isMoving)
        {
            newVelocity += rb.transform.forward * speed * movementInput.y * Time.deltaTime;
            Vector3 rotationDir = movementInput.y >= 0 ? transform.up : -transform.up;
            transform.Rotate(rotationSpeed * rotationDir * movementInput.x * Time.deltaTime);
        }
    }

    private void hanldeJumping(Vector3 oldVelocity, ref Vector3 newVelocity)
    {
        bool isOnGround = isGrounded();

        if (isJumping)
        {
            if (isOnGround)
            {
                newVelocity += rb.transform.up * jumpSpeed;
                animator.SetBool(isJumpingAnimatorId, isJumping);
            }
            isJumping = false;
        }

        if (!isOnGround)
        {
            newVelocity = oldVelocity;
            animator.SetBool(isFallingAnimatorId, true);
        }
        else
            animator.SetBool(isFallingAnimatorId, false);
    }


    void Update()
    {
        
        animator.SetBool(isJumpingAnimatorId, isJumping);
        
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private bool isGrounded()
    {
        return Physics.Raycast(rb.transform.position, -Vector3.up, 0.1f);
    }
}
