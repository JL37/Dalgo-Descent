using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Modifer")]
    [Range(0, 1)][SerializeField] private float runSpeed;
    [Range(0, 1)][SerializeField] private float slowSpeed;
    [Range(0, 1)][SerializeField] private float walkSpeed;
    [Range(0, 1)][SerializeField] private float turnSpeed;
    [SerializeField] private float jumpForce;

    [Header("Variables")]
    [SerializeField] private LayerMask GroundLayer;
    public Rigidbody PlayerRigidBody;
    public Animator PlayerAnimator;
    public PlayerInput InputScript;
    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public float Velocity { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }

    private int IsWalkingHash;
    private int JumpTriggerHash;
    private int IsLandingHash;
    private int IsGroundedHash;
    private int VelocityHash;
    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        IsWalkingHash = Animator.StringToHash("IsWalking");
        JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        IsLandingHash = Animator.StringToHash("IsLanding");
        IsGroundedHash = Animator.StringToHash("IsGrounded");
        VelocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        IsGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, GroundLayer) && !IsJump;
        //Set to Landing - Going Down + Not Grounded
        if (PlayerRigidBody.velocity.y < -0.0001f && !IsGrounded) 
        {
            IsJump = false;
            IsLanding = true;
        }
        //Set to Grounded
        else if (IsGrounded)
        {
            IsLanding = false;
            IsGrounded = true;
        }

        if (IsMoving)
        {
            Vector3 cameraForward = InputScript.camera.transform.forward;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            //Apply Camera Direction
            Rotation = Quaternion.LookRotation(cameraForward);

            //Apply Movement Direction based on Camera Direction
            Rotation = Rotation * Quaternion.LookRotation(MoveDirection);
        }
        else
        {
            Velocity = Mathf.Lerp(Velocity, 0, slowSpeed);
        }

        PlayerAnimator.SetBool(IsWalkingHash, IsMoving);
        PlayerAnimator.SetFloat(VelocityHash, Velocity);
        PlayerAnimator.SetBool(IsLandingHash, IsLanding);
        PlayerAnimator.SetBool(IsGroundedHash, IsGrounded);
    }

    void OnAnimatorMove()
    {
        float animationDelta = PlayerAnimator.deltaPosition.magnitude;

        if(IsGrounded)
            PlayerRigidBody.MovePosition(PlayerRigidBody.position + Rotation * Vector3.forward * animationDelta);
        else
            PlayerRigidBody.MovePosition(PlayerRigidBody.position + Rotation * Vector3.forward * Velocity * Time.fixedDeltaTime);

        PlayerRigidBody.MoveRotation(Quaternion.Slerp(PlayerRigidBody.rotation, Rotation, turnSpeed));
    }

    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        var contextDirection = context.ReadValue<Vector2>();
        IsMoving = !context.canceled;
        MoveDirection = IsMoving ? walkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;

        if (IsMoving && !IsRunning)
        {
            Velocity = walkSpeed;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        PlayerStats pStats = GetComponent<PlayerStats>();
        if (pStats && context.started)
        {
            if (pStats.GetChest())
            {
                pStats.GetChest().OnInteract();
                return;
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (IsMoving)
        {
            IsRunning = !context.canceled;
            Velocity = !context.canceled ? runSpeed : walkSpeed;
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control + " " + context.phase);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !IsJump && IsGrounded)
        {
            PlayerAnimator.SetTrigger(JumpTriggerHash);
            PlayerRigidBody.AddForce(Rotation * Vector3.forward * jumpForce * Velocity, ForceMode.Impulse);
            PlayerRigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            IsJump = true;
            IsGrounded = false;
        }
    }
    #endregion

    void OnEnable()
    {
        InputScript.ActivateInput();
    }

    void OnDisable()
    {
        InputScript.DeactivateInput();
    }
}
